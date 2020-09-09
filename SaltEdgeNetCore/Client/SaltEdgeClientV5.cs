using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using SaltEdgeNetCore.Client.Endpoints;
using SaltEdgeNetCore.Extension;
using SaltEdgeNetCore.Models.Account;
using SaltEdgeNetCore.Models.Assets;
using SaltEdgeNetCore.Models.Attempts;
using SaltEdgeNetCore.Models.Category;
using SaltEdgeNetCore.Models.Connections;
using SaltEdgeNetCore.Models.ConnectSession;
using SaltEdgeNetCore.Models.Consents;
using SaltEdgeNetCore.Models.Country;
using SaltEdgeNetCore.Models.Currencies;
using SaltEdgeNetCore.Models.Customer;
using SaltEdgeNetCore.Models.Error;
using SaltEdgeNetCore.Models.HolderInfo;
using SaltEdgeNetCore.Models.Merchant;
using SaltEdgeNetCore.Models.OAuthProvider;
using SaltEdgeNetCore.Models.Provider;
using SaltEdgeNetCore.Models.Rates;
using SaltEdgeNetCore.Models.Responses;
using SaltEdgeNetCore.Models.Transaction;
using SaltEdgeNetCore.SaltEdgeExceptions;

namespace SaltEdgeNetCore.Client
{
    public class SaltEdgeClientV5 : ISaltEdgeClientV5
    {
        private readonly SaltEdgeOptions _options;

        private static AsymmetricKeyParameter _privateKey;

        private static RsaKeyParameters _publicKey;

        public SaltEdgeClientV5(SaltEdgeOptions options = default)
        {
            _options = options;
            if (_options != null && (_options.LiveMode && string.IsNullOrWhiteSpace(_options.PrivateKeyPath)))
            {
                throw new PrivateKeyMissingException("Signing private key is missing");
            }

            if (_options != null && _options.LiveMode)
            {
                _privateKey = ReadPrivateKey(_options.PrivateKeyPath);
            }

            if (options != null && options.LiveMode && !string.IsNullOrWhiteSpace(options.PublicKeyPath))
            {
                _publicKey = ReadPublicKey(options.PublicKeyPath);
            }

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }


        public async Task<IEnumerable<SeCountry>> ListCountriesAsync()
        {
            using var client = Client();
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature", GenerateSignature(
                    WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.CountryList.Value)));
            }

            var response = await client.GetAsync(GenerateUrl(SaltEdgeEndpointsV5.CountryList.Value));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<SeCountry>>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeProvider> ProviderShowAsync(string providerCode)
        {
            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature", GenerateSignature(
                    WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Providers.Value}/{providerCode}")));
            }

            var response = await client
                .GetAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Providers.Value}/{providerCode}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeProvider>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeProvider>, SePaging>> ProvidersListAsync(DateTime? fromDate = null,
            string fromId = default,
            string countryCode = default, string mode = default,
            bool includeFakeProviders = false, bool includeProviderFields = false, string providerKeyOwner = default)
        {
            using var client = Client();
            var appendedToUrl = false;
            var url = new StringBuilder(SaltEdgeEndpointsV5.Providers.Value);
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                url.Append($"?country_code={countryCode.ToUpper()}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
                appendedToUrl = true;
            }

            if (fromDate != null)
            {
                url.Append(appendedToUrl ? $"&from_date={fromDate}" : $"?from_date={fromDate}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(mode))
            {
                if (mode == "oauth" && mode == "web" && mode == "api" && mode == "file")
                {
                    throw new InvalidArgumentException("mode parameter should be 'oauth', 'web', 'api' or 'file'");
                }

                url.Append(appendedToUrl ? $"&mode={mode}" : $"?mode={mode}");
                appendedToUrl = true;
            }

            if (includeFakeProviders)
            {
                url.Append(appendedToUrl ? "&include_fake_providers=true" : "?include_fake_providers=true");
                appendedToUrl = true;
            }

            if (includeProviderFields)
            {
                url.Append(appendedToUrl ? "&include_provider_fields=true" : "?include_provider_fields=true");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(providerKeyOwner))
            {
                if (providerKeyOwner != "client" && providerKeyOwner != "saltedge")
                {
                    throw new InvalidArgumentException("providerKeyOwner parameter should be 'client' or 'saltedge'");
                }

                url.Append(appendedToUrl
                    ? $"&provider_key_owner={providerKeyOwner}"
                    : $"?provider_key_owner={providerKeyOwner}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt, GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeProvider>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeCustomer> CreateCustomerAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            using var client = Client();

            var body = new
            {
                data = new
                {
                    identifier = customerId
                }
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Customers.Value), body));
            }

            var response = await client
                .PostAsync(GenerateUrl(SaltEdgeEndpointsV5.Customers.Value), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeCustomer>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeCustomer> CustomerShowAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id ");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}")));
            }

            var response = await client
                .GetAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeCustomer>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeCustomer>, SePaging>> CustomersListAsync(string fromId = default,
            string nextId = default)
        {
            var url = new StringBuilder(SaltEdgeEndpointsV5.Customers.Value);
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(nextId))
            {
                url.Append($"?next_id={nextId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeCustomer>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<RemoveCustomer> CustomerRemoveAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature("DELETE", expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}")));
            }

            var response = await client
                .DeleteAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<RemoveCustomer>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<LockCustomer> CustomerLockAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/lock")));
            }

            var response = await client
                .PutAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/lock"), null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<LockCustomer>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<UnlockCustomer> CustomerUnlockAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/unlock")));
            }

            var response = await client
                .PutAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/unlock"), null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<UnlockCustomer>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SessionResponse> SessionCreateAsync(CreateSession session)
        {
            if (session == null || string.IsNullOrWhiteSpace(session.CustomerId))
            {
                throw new InvalidArgumentException(
                    "Invalid argument please visit salt edge documentation: " +
                    "https://docs.saltedge.com/account_information/v5/#connect_sessions-create");
            }

            var body = new
            {
                data = session
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/create"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/create"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SessionResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SessionResponse> SessionReconnectAsync(ReconnectSession session)
        {
            if (session == null || string.IsNullOrWhiteSpace(session.ConnectionId))
            {
                throw new InvalidArgumentException(
                    "Invalid argument please visit salt edge documentation: " +
                    "https://docs.saltedge.com/account_information/v5/#connect_sessions-reconnect");
            }

            var body = new
            {
                data = session
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/reconnect"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/reconnect"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SessionResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SessionResponse> SessionRefreshAsync(RefreshSession session)
        {
            if (session == null || string.IsNullOrWhiteSpace(session.ConnectionId))
            {
                throw new InvalidArgumentException(
                    "Invalid argument please visit salt edge documentation: " +
                    "https://docs.saltedge.com/account_information/v5/#connect_sessions-reconnect");
            }

            var body = new
            {
                data = session
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/refresh"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/refresh"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SessionResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<OAuthProviderResponse> OAuthProviderCreateAsync(CreateOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null CreateOAuthProvider object");
            }

            using var client = Client();

            var body = new
            {
                date = oAuthProvider
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/create"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/create"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<OAuthProviderResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<OAuthProviderResponse> OAuthProviderReconnectAsync(ReconnectOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null ReconnectOAuthProvider object");
            }

            using var client = Client();

            var body = new
            {
                date = oAuthProvider
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/reconnect"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/reconnect"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<OAuthProviderResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<AuthorizeOAuthProviderResponse> AuthProviderAuthorizeAsync(
            AuthorizeOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null AuthorizeOAuthProvider object");
            }

            using var client = Client();

            var body = new
            {
                data = oAuthProvider
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/authorize"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/authorize"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse =
                    JsonConvert.DeserializeObject<SimpleResponse<AuthorizeOAuthProviderResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeConnection>, SePaging>> ConnectionsListAsync(string customerId,
            string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            var url = new StringBuilder(SaltEdgeEndpointsV5.Connections.Value);

            url.Append($"?customer_id={customerId}");

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append($"&from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeConnection>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeConnection> ConnectionShowAsync(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}")));
            }

            var response = await client
                .GetAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeConnection>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<RemoveConnection> ConnectionRemoveAsync(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature("DELETE", expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}")));
            }

            var response = await client
                .DeleteAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<RemoveConnection>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeHolderInfo> HolderInfoShowAsync(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.HolderInfo.Value}?connection_id={connectionId}")));
            }

            var response = await client
                .GetAsync(GenerateUrl($"{SaltEdgeEndpointsV5.HolderInfo.Value}?connection_id={connectionId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeHolderInfo>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeAttempt>, SePaging>> AttemptsListAsync(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Attempts.Value}?connection_id={connectionId}")));
            }

            var response = await client
                .GetAsync(GenerateUrl($"{SaltEdgeEndpointsV5.Attempts.Value}?connection_id={connectionId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeAttempt>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeAttempt> AttemptShowAsync(string connectionId, string attemptId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (string.IsNullOrWhiteSpace(attemptId))
            {
                throw new InvalidArgumentException("Null attempt id");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Attempts.Value}/{attemptId}?connection_id={connectionId}")));
            }

            var response = await client
                .GetAsync(GenerateUrl(
                    $"{SaltEdgeEndpointsV5.Attempts.Value}/{attemptId}?connection_id={connectionId}"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeAttempt>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeAccount>, SePaging>> AccountListAsync(string connectionId,
            string customerId = default,
            string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null");
            }

            var url = new StringBuilder(SaltEdgeEndpointsV5.Accounts.Value);
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeAccount>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsListAsync(
            string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var url = new StringBuilder(SaltEdgeEndpointsV5.Transactions.Value);

            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append($"&from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse =
                    JsonConvert.DeserializeObject<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsDuplicatedListAsync(
            string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicates");
            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append($"&from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse =
                    JsonConvert.DeserializeObject<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SaltEdgeTransaction>, SePaging>> TransactionsPendingListAsync(
            string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Transactions.Value}/pending");

            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append($"&from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse =
                    JsonConvert.DeserializeObject<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<DuplicatedResponse> TransactionsDuplicateAsync(string customerId,
            IEnumerable<string> transactionIds)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            var enumerable = transactionIds as string[] ?? transactionIds.ToArray();

            if (transactionIds == null || !enumerable.Any())
            {
                throw new InvalidArgumentException("Null or empty transaction id's list");
            }

            var body = new
            {
                data = new
                {
                    customer_id = customerId,
                    transaction_ids = enumerable
                }
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicate"), body));
            }

            var response = await client
                .PutAsync(GenerateUrl(
                    $"{SaltEdgeEndpointsV5.Transactions.Value}/duplicate"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<DuplicatedResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<UnDuplicatedResponse> TransactionsUnDuplicateAsync(string customerId,
            IEnumerable<string> transactionIds)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            var enumerable = transactionIds as string[] ?? transactionIds.ToArray();

            if (transactionIds == null || !enumerable.Any())
            {
                throw new InvalidArgumentException("Null or empty transaction id's list");
            }

            var body = new
            {
                data = new
                {
                    customer_id = customerId,
                    transaction_ids = enumerable
                }
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Transactions.Value}/unduplicate"), body));
            }

            var response = await client
                .PutAsync(GenerateUrl(
                    $"{SaltEdgeEndpointsV5.Transactions.Value}/unduplicate"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<UnDuplicatedResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<RemoveTransactionResponse> TransactionRemoveAsync(string customerId, string accountId,
            int keepDays = 0)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                throw new InvalidArgumentException("Null account id");
            }

            var body = new
            {
                data = new
                {
                    customer_id = customerId,
                    account_id = accountId,
                    keep_days = keepDays
                }
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature("DELETE", expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Transactions.Value), body));
            }

            var response = await client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                Content = GetBody(body),
                RequestUri = new Uri(GenerateUrl(SaltEdgeEndpointsV5.Transactions.Value))
            });

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<RemoveTransactionResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Response<IEnumerable<SeConsent>, SePaging>> ConsentsListAsync(string connectionId = default,
            string customerId = default, string fromId = default)
        {
            var url = new StringBuilder(SaltEdgeEndpointsV5.Consents.Value);
            var appendedToUrl = false;

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null");
            }

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<Response<IEnumerable<SeConsent>, SePaging>>(content);
                return apiResponse;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeConsent> ConsentShowAsync(string id, string connectionId = default,
            string customerId = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidArgumentException("Null consent id");
            }

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null," +
                                                   " please visit the documentation : https://docs.saltedge.com/account_information/v5/#consents-show");
            }

            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Consents.Value}/{id}");
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeConsent>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<SeConsent> ConsentRevokeAsync(string id, string connectionId = default,
            string customerId = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidArgumentException("Null consent id");
            }

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null," +
                                                   " please visit the documentation : https://docs.saltedge.com/account_information/v5/#consents-revoke");
            }

            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Consents.Value}/{id}/revoke");
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .PutAsync(GenerateUrl(url.ToString()), null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<SeConsent>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<IDictionary<string, IEnumerable<string>>> CategoryListAsync()
        {
            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Categories.Value)));
            }

            var response = await client
                .GetAsync(GenerateUrl(SaltEdgeEndpointsV5.Categories.Value));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                return ProcessCategories(content);
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<CategoryLearnResponse> CategoryLearnAsync(string customerId,
            IEnumerable<SeCategoryLearn> transactionsList)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new IdentifierInvalidException("Null customer id");
            }

            var categoryLearns = transactionsList as SeCategoryLearn[] ?? transactionsList.ToArray();
            if (categoryLearns.Any(tLearn =>
                string.IsNullOrWhiteSpace(tLearn.Id) || string.IsNullOrWhiteSpace(tLearn.CategoryCode)))
            {
                throw new InvalidArgumentException("Null transaction id or category code," +
                                                   "please visit the documentation: https://docs.saltedge.com/account_information/v5/#categories-learn");
            }

            var body = new
            {
                data = new
                {
                    customer_id = customerId,
                    transactions = categoryLearns
                }
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl($"{SaltEdgeEndpointsV5.Categories.Value}/learn"), body));
            }

            var response = await client
                .PostAsync(GenerateUrl(
                    $"{SaltEdgeEndpointsV5.Categories.Value}/learn"), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<CategoryLearnResponse>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<IEnumerable<SeCurrency>> CurrenciesAsync()
        {
            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Currencies.Value)));
            }

            var response = await client
                .GetAsync(GenerateUrl(
                    SaltEdgeEndpointsV5.Currencies.Value));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<SeCurrency>>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<IEnumerable<SeAsset>> AssetsAsync()
        {
            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Assets.Value)));
            }

            var response = await client
                .GetAsync(GenerateUrl(
                    SaltEdgeEndpointsV5.Assets.Value));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<SeAsset>>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<IEnumerable<SeRate>> RatesAsync(DateTime? date = null)
        {
            var url = new StringBuilder(SaltEdgeEndpointsV5.Rates.Value);
            if (date != null)
            {
                url.Append($"?date={date.ToString(Config.DateFormat)}");
            }

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                        GenerateUrl(url.ToString())));
            }

            var response = await client
                .GetAsync(GenerateUrl(url.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<SeRate>>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<IEnumerable<Merchant>> MerchantsListAsync(IEnumerable<string> merchantIds)
        {
            var ids = merchantIds as string[] ?? merchantIds.ToArray();
            if (merchantIds == null || !ids.Any())
            {
                throw new InvalidArgumentException("Merchant ids list is null or empty");
            }

            var body = new
            {
                data = ids
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), body));
            }

            var response = await client
                .PostAsync(GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<Merchant>>>(content);
                return apiResponse.Data;
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        public async Task<Merchant> MerchantShowAsync(string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new InvalidArgumentException("Null merchant id");
            }

            var body = new
            {
                data = new[] {merchantId}
            };

            using var client = Client();

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Expires-at", expireAt);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Signature",
                    GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                        GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), body));
            }

            var response = await client
                .PostAsync(GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), GetBody(body));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                HandleError(content);
            }

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<SimpleResponse<IEnumerable<Merchant>>>(content);
                return apiResponse.Data.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception($"{typeof(ISaltEdgeClientV5)}: Deserialization Exception \n {e.Message}");
            }
        }

        private static IDictionary<string, IEnumerable<string>> ProcessCategories(string content)
        {
            var categoryList = new Dictionary<string, IEnumerable<string>>();
            var categories = JsonConvert.DeserializeObject<JObject>(content);
            foreach (var p in categories.Properties())
            {
                var prop = JsonConvert.DeserializeObject<JObject>(p.Value.ToString());
                foreach (var x in prop.Properties())
                {
                    var cat = JsonConvert.DeserializeObject<JObject>(x.Value.ToString());
                    foreach (var d in cat.Properties())
                    {
                        if (!categoryList.ContainsKey(d.Name))
                        {
                            categoryList.Add(d.Name,
                                JsonConvert.DeserializeObject<IEnumerable<string>>(d.Value.ToString()));
                        }
                    }
                }
            }

            return categoryList;
        }

        private static void HandleError(string content)
        {
            var error = JsonConvert.DeserializeObject<SaltEdgeError>(content);
            ErrorHandler.Handle(error.Error.Class, error.Error.Message);
        }
        
        private HttpClient Client()
        {
            var client = new HttpClient();

            if (_options.LiveMode)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("App-id", _options.AppId);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Secret", _options.Secret);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
        
        private int GenerateExpiresAt()
        {
            if (_options.WithExpiration == 0)
            {
                _options.WithExpiration = 1;
            }

            var unixBegin = new DateTime(1970, 1, 1);
            return (int) DateTime.UtcNow.AddMinutes(_options.WithExpiration).Subtract(unixBegin).TotalSeconds;
        }

        private static string GenerateSignature(string method, string expires, string url, object body = default)
        {
            var serializedBody = string.Empty;
            if (body != null)
            {
                serializedBody = JsonConvert.SerializeObject(body);
            }

            var signature = $"{expires}|{method}|{url}|{serializedBody}";
            var bytes = Encoding.UTF8.GetBytes(signature);
            var shaSignature = Sign(bytes);

            return Convert.ToBase64String(shaSignature);
        }

        private static byte[] Sign(byte[] bytes)
        {
            var sig = SignerUtilities.GetSigner("SHA256withRSA");
            sig.Init(true, _privateKey);
            sig.BlockUpdate(bytes, 0, bytes.Length);
            return sig.GenerateSignature();
        }

        private static AsymmetricKeyParameter ReadPrivateKey(string privateKeyFileName)
        {
            using var reader = File.OpenText(privateKeyFileName);
            var keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();

            return keyPair.Private;
        }


        private static RsaKeyParameters ReadPublicKey(string fileName)
        {
            using var reader = File.OpenText(fileName);
            var keyPair = (RsaKeyParameters) new PemReader(reader).ReadObject();

            return keyPair;
        }

        public bool Verify(string url, string requestBody, string signature)
        {
            var sig = SignerUtilities.GetSigner("SHA256withRSA");
            sig.Init(false, _publicKey);

            var input = $"{url}|{requestBody}";
            var bytes = Encoding.UTF8.GetBytes(input);

            var decodedSignature = Convert.FromBase64String(signature);

            sig.BlockUpdate(bytes, 0, bytes.Length);
            return sig.VerifySignature(decodedSignature);
        }

        private static string GenerateUrl(string endpoint) => $"https://www.saltedge.com/api/v5/{endpoint}";

        private static StringContent GetBody(object o)
            => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8);
    }
}