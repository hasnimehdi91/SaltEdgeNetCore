using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using SaltEdgeNetCore.Client.Endpoints;
using SaltEdgeNetCore.Extension;
using SaltEdgeNetCore.Models.Connections;
using SaltEdgeNetCore.Models.ConnectSession;
using SaltEdgeNetCore.Models.Country;
using SaltEdgeNetCore.Models.Customer;
using SaltEdgeNetCore.Models.Error;
using SaltEdgeNetCore.Models.OAuthProvider;
using SaltEdgeNetCore.Models.Provider;
using SaltEdgeNetCore.Models.Responses;
using SaltEdgeNetCore.SaltEdgeExceptions;

namespace SaltEdgeNetCore.Client
{
    public class SaltEdgeClientV5 : ISaltEdgeClient
    {
        private IDictionary<string, string> _headers;

        private IRestClient _client;

        public SaltEdgeClientV5()
        {
            _headers = new Dictionary<string, string>();
            _client = GetClient();
        }

        public ISaltEdgeClient SetHeaders(IDictionary<string, string> headers)
        {
            _headers = headers;
            _client = GetClient();
            return this;
        }

        public IEnumerable<Country> ListCountries()
        {
            var apiResponse = _client
                .Get<SimpleResponse<IEnumerable<Country>>>(new RestRequest(SaltEdgeEndpointsV5.CountryList.Value));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Provider ProviderShow(string providerCode)
        {
            var apiResponse = _client
                .Get<SimpleResponse<Provider>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Providers.Value}/{providerCode}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<Provider>, Paging> ProvidersList(DateTime? fromDate = null, string fromId = default,
            string countryCode = default, string mode = default,
            bool includeFakeProviders = false, bool includeProviderFields = false, string providerKeyOwner = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Providers.Value);
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                request.AddQueryParameter("country_code", "KW", true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
            }

            if (fromDate != null)
            {
                request.AddQueryParameter("from_date", fromDate.ToString(Config.DateFormat), true);
            }

            if (!string.IsNullOrWhiteSpace(mode))
            {
                if (mode != "oauth" || mode != "web" || mode != "api" || mode != "file")
                {
                    throw new InvalidArgumentException("mode parameter should be 'oauth', 'web', 'api' or 'file'");
                }

                request.AddQueryParameter("mode", mode, true);
            }

            if (includeFakeProviders)
            {
                request.AddQueryParameter("include_fake_providers", "true", true);
            }

            if (includeProviderFields)
            {
                request.AddQueryParameter("include_provider_fields", "true", true);
            }

            if (!string.IsNullOrWhiteSpace(providerKeyOwner))
            {
                if (providerKeyOwner != "client" || providerKeyOwner != "saltedge")
                {
                    throw new InvalidArgumentException("providerKeyOwner parameter should be 'client' or 'saltedge'");
                }

                request.AddQueryParameter("provider_key_owner", providerKeyOwner, true);
            }


            var apiResponse = _client.Get<Response<IEnumerable<Provider>, Paging>>(request);
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Customer CreateCustomer(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Missing customer id");
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Customers.Value);
            request.AddJsonBody(new
            {
                data = new
                {
                    identifier = customerId
                }
            });
            var apiResponse = _client.Post<SimpleResponse<Customer>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Customer CustomerShow(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("customer id is null");
            }

            var apiResponse =
                _client.Get<SimpleResponse<Customer>>(
                    new RestRequest($"{SaltEdgeEndpointsV5.Customers.Value}/{customerId}"));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<Customer>, Paging> CustomersList(string fromId = default, string nextId = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Customers.Value);

            if (!string.IsNullOrWhiteSpace(nextId))
            {
                request.AddQueryParameter("next_id", nextId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", nextId, true);
            }

            var apiResponse =
                _client.Get<Response<IEnumerable<Customer>, Paging>>(
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
                throw new InvalidArgumentException("customer id is null");
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
                throw new InvalidArgumentException("customer id is null");
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
                throw new InvalidArgumentException("customer id is null");
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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/create");
            request.AddJsonBody(new
            {
                data = session
            });

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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.ConnectSessions.Value}/reconnect");
            request.AddJsonBody(new
            {
                data = session
            });

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

            request.AddJsonBody(new
            {
                data = session
            });

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

        public Response<IEnumerable<Connection>, Paging> ConnectionsList(string customerId, string fromId = default)
        {
            throw new NotImplementedException();
        }

        public Connection ConnectionShow(string connectionId)
        {
            throw new NotImplementedException();
        }

        private static void HandleError(string content)
        {
            var error = JsonConvert.DeserializeObject<SaltEdgeError>(content);
            ErrorHandler.Handle(error.Error.Class, error.Error.Message);
        }

        private IRestClient GetClient()
        {
            _headers.Add("Content-Type", "application/json");

            var client = new RestClient(" https://www.saltedge.com/api/v5/");
            foreach (var (key, value) in _headers)
            {
                client.AddDefaultHeader(key, value);
            }

            client.UseNewtonsoftJson();
            return client;
        }
    }
}