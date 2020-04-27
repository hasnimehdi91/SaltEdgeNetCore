using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
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
        private IRestClient _client;

        private readonly SaltEdgeOptions _options;

        private static AsymmetricKeyParameter _privateKey;

        public SaltEdgeClientV5(SaltEdgeOptions options = default)
        {
            _options = options;
            _client = GetClient();
            if (_options.LiveMode && string.IsNullOrWhiteSpace(_options.PrivateKeyPath))
            {
                throw new PrivateKeyMissingException("Signing private key is missing");
            }

            if (_options.LiveMode)
            {
                _privateKey = ReadPrivateKey(_options.PrivateKeyPath);
            }

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }


        public IEnumerable<SeCountry> ListCountries()
        {
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.CountryList.Value)));
            }

            var apiResponse = _client
                .Get<SimpleResponse<IEnumerable<SeCountry>>>(new RestRequest(SaltEdgeEndpointsV5.CountryList.Value));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeProvider ProviderShow(string providerCode)
        {
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Providers.Value}/{providerCode}")));
            }

            var apiResponse = _client
                .Get<SimpleResponse<SeProvider>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Providers.Value}/{providerCode}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeProvider>, SePaging> ProvidersList(DateTime? fromDate = null,
            string fromId = default,
            string countryCode = default, string mode = default,
            bool includeFakeProviders = false, bool includeProviderFields = false, string providerKeyOwner = default)
        {
            var appendedToUrl = false;
            var url = new StringBuilder(SaltEdgeEndpointsV5.Providers.Value);
            var request = new RestRequest(SaltEdgeEndpointsV5.Providers.Value);
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                request.AddQueryParameter("country_code", countryCode.ToUpper(), true);
                url.Append($"?country_code={countryCode.ToUpper()}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
                appendedToUrl = true;
            }

            if (fromDate != null)
            {
                request.AddQueryParameter("from_date", fromDate.ToString(Config.DateFormat), true);
                url.Append(appendedToUrl ? $"&from_date={fromDate}" : $"?from_date={fromDate}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(mode))
            {
                if (mode == "oauth" && mode == "web" && mode == "api" && mode == "file")
                {
                    throw new InvalidArgumentException("mode parameter should be 'oauth', 'web', 'api' or 'file'");
                }

                request.AddQueryParameter("mode", mode, true);
                url.Append(appendedToUrl ? $"&mode={mode}" : $"?mode={mode}");
                appendedToUrl = true;
            }

            if (includeFakeProviders)
            {
                request.AddQueryParameter("include_fake_providers", "true", true);
                url.Append(appendedToUrl ? "&include_fake_providers=true" : "?include_fake_providers=true");
                appendedToUrl = true;
            }

            if (includeProviderFields)
            {
                request.AddQueryParameter("include_provider_fields", "true", true);
                url.Append(appendedToUrl ? "&include_provider_fields=true" : "?include_provider_fields=true");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(providerKeyOwner))
            {
                if (providerKeyOwner != "client" && providerKeyOwner != "saltedge")
                {
                    throw new InvalidArgumentException("providerKeyOwner parameter should be 'client' or 'saltedge'");
                }

                request.AddQueryParameter("provider_key_owner", providerKeyOwner, true);
                url.Append(appendedToUrl
                    ? $"&provider_key_owner={providerKeyOwner}"
                    : $"?provider_key_owner={providerKeyOwner}");
            }

            Console.WriteLine(GenerateUrl(url.ToString()));
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client,
                    GenerateSignature(WebRequestMethods.Http.Get, expireAt, GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SeProvider>, SePaging>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeCustomer CreateCustomer(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

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
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Customers.Value), body));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Customers.Value);
            request.AddJsonBody(body);
            var apiResponse = _client.Post<SimpleResponse<SeCustomer>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeCustomer CustomerShow(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id ");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}")));
            }

            var apiResponse =
                _client.Get<SimpleResponse<SeCustomer>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeCustomer>, SePaging> CustomersList(string fromId = default,
            string nextId = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Customers.Value);
            var url = new StringBuilder(SaltEdgeEndpointsV5.Customers.Value);
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(nextId))
            {
                request.AddQueryParameter("next_id", nextId, true);
                url.Append($"?next_id={nextId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse =
                _client.Get<Response<IEnumerable<SeCustomer>, SePaging>>(
                    request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public RemoveCustomer CustomerRemove(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature("DELETE", expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}")));
            }

            var apiResponse =
                _client.Delete<SimpleResponse<RemoveCustomer>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public LockCustomer CustomerLock(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/lock")));
            }

            var apiResponse =
                _client.Put<SimpleResponse<LockCustomer>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/lock"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public UnlockCustomer CustomerUnlock(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/unlock")));
            }

            var apiResponse =
                _client.Put<SimpleResponse<UnlockCustomer>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}/unlock"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SessionResponse SessionCreate(CreateSession session)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/create"), body));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/create");
            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<SessionResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SessionResponse SessionReconnect(ReconnectSession session)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/reconnect"), body));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/reconnect");
            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<SessionResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SessionResponse SessionRefresh(RefreshSession session)
        {
            if (session == null || string.IsNullOrWhiteSpace(session.ConnectionId))
            {
                throw new InvalidArgumentException(
                    "Invalid argument please visit salt edge documentation: " +
                    "https://docs.saltedge.com/account_information/v5/#connect_sessions-reconnect");
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/refresh");

            var body = new
            {
                data = session
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/refresh"), body));
            }

            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<SessionResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public OAuthProviderResponse OAuthProviderCreate(CreateOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null CreateOAuthProvider object");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/create"), oAuthProvider));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.OauthProviders.Value}/create");
            request.AddJsonBody(oAuthProvider);

            var apiResponse =
                _client.Post<SimpleResponse<OAuthProviderResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public OAuthProviderResponse OAuthProviderReconnect(ReconnectOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null ReconnectOAuthProvider object");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/reconnect"), oAuthProvider));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.OauthProviders.Value}/reconnect");
            request.AddJsonBody(oAuthProvider);

            var apiResponse =
                _client.Post<SimpleResponse<OAuthProviderResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public AuthorizeOAuthProviderResponse AuthProviderAuthorize(AuthorizeOAuthProvider oAuthProvider)
        {
            if (oAuthProvider == null)
            {
                throw new InvalidArgumentException("Null AuthorizeOAuthProvider object");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.OauthProviders.Value}/authorize"), oAuthProvider));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.OauthProviders.Value}/authorize");
            request.AddJsonBody(oAuthProvider);

            var apiResponse =
                _client.Put<SimpleResponse<AuthorizeOAuthProviderResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeConnection>, SePaging> ConnectionsList(string customerId, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Connections.Value);
            var url = new StringBuilder(SaltEdgeEndpointsV5.Connections.Value);

            request.AddQueryParameter("customer_id", customerId, true);
            url.Append($"?customer_id={customerId}");

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append($"&from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SeConnection>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeConnection ConnectionShow(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}")));
            }

            var apiResponse =
                _client.Get<SimpleResponse<SeConnection>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public RemoveConnection ConnectionRemove(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature("DELETE", expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}")));
            }

            var apiResponse =
                _client.Delete<SimpleResponse<RemoveConnection>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Connections.Value}/{connectionId}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeHolderInfo HolderInfoShow(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.HolderInfo.Value}?connection_id={connectionId}")));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.HolderInfo.Value);
            request.AddQueryParameter("connection_id", connectionId, true);

            var apiResponse = _client.Get<SimpleResponse<SeHolderInfo>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeAttempt>, SePaging> AttemptsList(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Attempts.Value}?connection_id={connectionId}")));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Attempts.Value);
            request.AddQueryParameter("connection_id", connectionId, true);

            var apiResponse = _client.Get<Response<IEnumerable<SeAttempt>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeAttempt AttemptShow(string connectionId, string attemptId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            if (string.IsNullOrWhiteSpace(attemptId))
            {
                throw new InvalidArgumentException("Null attempt id");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Attempts.Value}/{attemptId}?connection_id={connectionId}")));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Attempts.Value}/{attemptId}");
            request.AddQueryParameter("connection_id", connectionId, true);
            var apiResponse = _client.Get<SimpleResponse<SeAttempt>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeAccount>, SePaging> AccountList(string connectionId, string customerId = default,
            string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null");
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Accounts.Value);
            var url = new StringBuilder(SaltEdgeEndpointsV5.Accounts.Value);
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SeAccount>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);

            return null;
        }

        public Response<IEnumerable<SaltEdgeTransaction>, SePaging> TransactionsList(string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Transactions.Value);

            var url = new StringBuilder(SaltEdgeEndpointsV5.Transactions.Value);

            request.AddQueryParameter("connection_id", connectionId, true);
            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append($"&from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SaltEdgeTransaction>, SePaging> TransactionsDuplicatedList(string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicates");
            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicates");
            request.AddQueryParameter("connection_id", connectionId, true);
            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append($"&from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SaltEdgeTransaction>, SePaging> TransactionsPendingList(string connectionId,
            string accountId = default, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Null connection id");
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/pending");
            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Transactions.Value}/pending");
            request.AddQueryParameter("connection_id", connectionId, true);
            url.Append($"?connection_id={connectionId}");

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
                url.Append($"&account_id={accountId}");
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append($"&from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SaltEdgeTransaction>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public DuplicatedResponse TransactionsDuplicate(string customerId, IEnumerable<string> transactionIds)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicate"), body));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicate");

            request.AddJsonBody(body);

            var apiResponse = _client.Put<SimpleResponse<DuplicatedResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public UnDuplicatedResponse TransactionsUnDuplicate(string customerId, IEnumerable<string> transactionIds)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Transactions.Value}/unduplicate"), body));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/unduplicate");

            request.AddJsonBody(body);

            var apiResponse = _client.Put<SimpleResponse<UnDuplicatedResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public RemoveTransactionResponse TransactionRemove(string customerId, string accountId, int keepDays = 0)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature("DELETE", expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Transactions.Value), body));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Transactions.Value);

            request.AddJsonBody(body);

            var apiResponse = _client.Delete<RemoveTransactionResponse>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeConsent>, SePaging> ConsentsList(string connectionId = default,
            string customerId = default, string fromId = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Consents.Value);
            var url = new StringBuilder(SaltEdgeEndpointsV5.Consents.Value);
            var appendedToUrl = false;

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null");
            }

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
                url.Append(appendedToUrl ? $"&from_id={fromId}" : $"?from_id={fromId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<Response<IEnumerable<SeConsent>, SePaging>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeConsent ConsentShow(string id, string connectionId = default, string customerId = default)
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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Consents.Value}/{id}");
            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Consents.Value}/{id}");
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Get<SimpleResponse<SeConsent>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public SeConsent ConsentRevoke(string id, string connectionId = default, string customerId = default)
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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Consents.Value}/{id}/revoke");
            var url = new StringBuilder($"{SaltEdgeEndpointsV5.Consents.Value}/{id}/revoke");
            var appendedToUrl = false;

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
                url.Append($"?connection_id={connectionId}");
                appendedToUrl = true;
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
                url.Append(appendedToUrl ? $"&customer_id={customerId}" : $"?customer_id={customerId}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Put, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse = _client.Put<SimpleResponse<SeConsent>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public IDictionary<string, IEnumerable<string>> CategoryList()
        {
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Categories.Value)));
            }

            var apiResponse = _client.Get(new RestRequest(SaltEdgeEndpointsV5.Categories.Value));
            if (apiResponse.IsSuccessful)
            {
                return ProcessCategories(apiResponse);
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public CategoryLearnResponse CategoryLearn(string customerId, IEnumerable<SeCategoryLearn> transactionsList)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl($"{SaltEdgeEndpointsV5.Categories.Value}/learn"), body));
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Categories.Value}/learn");

            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<CategoryLearnResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public IEnumerable<SeCurrency> Currencies()
        {
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Currencies.Value)));
            }

            var apiResponse =
                _client.Get<SimpleResponse<IEnumerable<SeCurrency>>>(
                    new RestRequest(SaltEdgeEndpointsV5.Currencies.Value));

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public IEnumerable<SeAsset> Assets()
        {
            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Assets.Value)));
            }

            var apiResponse =
                _client.Get<SimpleResponse<IEnumerable<SeAsset>>>(
                    new RestRequest(SaltEdgeEndpointsV5.Assets.Value));

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public IEnumerable<SeRate> Rates(DateTime? date = null)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Rates.Value);
            var url = new StringBuilder(SaltEdgeEndpointsV5.Rates.Value);
            if (date != null)
            {
                request.AddQueryParameter("date", date.ToString(Config.DateFormat), true);
                url.Append($"?date={date.ToString(Config.DateFormat)}");
            }

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Get, expireAt,
                    GenerateUrl(url.ToString())));
            }

            var apiResponse =
                _client.Get<SimpleResponse<IEnumerable<SeRate>>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public IEnumerable<Merchant> MerchantsList(IEnumerable<string> merchantIds)
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

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), body));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Merchants.Value);

            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<IEnumerable<Merchant>>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Merchant MerchantShow(string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new InvalidArgumentException("Null merchant id");
            }

            var body = new
            {
                data = new[] {merchantId}
            };

            if (_options.LiveMode)
            {
                var expireAt = GenerateExpiresAt().ToString();
                _client = AddExpireAt(_client, expireAt);
                _client = AddSignature(_client, GenerateSignature(WebRequestMethods.Http.Post, expireAt,
                    GenerateUrl(SaltEdgeEndpointsV5.Merchants.Value), body));
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Merchants.Value);

            request.AddJsonBody(body);

            var apiResponse = _client.Post<SimpleResponse<IEnumerable<Merchant>>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data.FirstOrDefault();
            }

            HandleError(apiResponse.Content);
            return null;
        }

        private static IDictionary<string, IEnumerable<string>> ProcessCategories(IRestResponse result)
        {
            var categoryList = new Dictionary<string, IEnumerable<string>>();
            var categories = JsonConvert.DeserializeObject<JObject>(result.Content);
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

        private IRestClient GetClient()
        {
            if (string.IsNullOrWhiteSpace(_options.AppId))
            {
                throw new AppIdMissingException("Salt Edge App-Id is missing");
            }

            if (string.IsNullOrWhiteSpace(_options.Secret))
            {
                throw new SecretMissingException("Salt Edge Secret is missing");
            }

            if (_options.LiveMode)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            var client = new RestClient("https://www.saltedge.com/api/v5/");
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultHeader("App-id", _options.AppId);
            client.AddDefaultHeader("Secret", _options.Secret);

            client.UseNewtonsoftJson();
            return client;
        }

        private IRestClient AddExpireAt(IRestClient client, string expireAt)
        {
            if (!_options.LiveMode) return client;
            if (!string.IsNullOrWhiteSpace(_options.PrivateKeyPath))
            {
                _client.RemoveDefaultParameter("Expires-at");
                client.AddDefaultHeader("Expires-at", expireAt);
            }
            else
            {
                throw new PrivateKeyMissingException("Signing key is missing");
            }

            return client;
        }

        private IRestClient AddSignature(IRestClient client, string signature)
        {
            if (!_options.LiveMode) return client;
            _client.RemoveDefaultParameter("Signature");
            client.AddDefaultHeader("Signature", signature);
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
            AsymmetricCipherKeyPair keyPair;

            using (var reader = File.OpenText(privateKeyFileName))
                keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();

            return keyPair.Private;
        }

        private static string GenerateUrl(string endpoint)
        {
            return $"https://www.saltedge.com/api/v5/{endpoint}";
        }
    }
}