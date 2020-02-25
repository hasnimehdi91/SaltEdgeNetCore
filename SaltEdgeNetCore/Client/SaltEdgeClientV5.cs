using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private IDictionary<string, string> _headers;

        private IRestClient _client;

        public SaltEdgeClientV5()
        {
            _headers = new Dictionary<string, string>();
            _client = GetClient();
        }

        public ISaltEdgeClientV5 SetHeaders(IDictionary<string, string> headers)
        {
            _headers = headers;
            _client = GetClient();
            return this;
        }

        public IEnumerable<SeCountry> ListCountries()
        {
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

        public Response<IEnumerable<SeProvider>, SePaging> ProvidersList(DateTime? fromDate = null, string fromId = default,
            string countryCode = default, string mode = default,
            bool includeFakeProviders = false, bool includeProviderFields = false, string providerKeyOwner = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Providers.Value);
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                request.AddQueryParameter("country_code", countryCode, true);
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

            var request = new RestRequest(SaltEdgeEndpointsV5.Customers.Value);
            request.AddJsonBody(new
            {
                data = new
                {
                    identifier = customerId
                }
            });
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

        public Response<IEnumerable<SeCustomer>, SePaging> CustomersList(string fromId = default, string nextId = default)
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

        public Response<IEnumerable<SeConnection>, SePaging> ConnectionsList(string customerId, string fromId = default)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidArgumentException("Null customer id");
            }

            var request = new RestRequest(SaltEdgeEndpointsV5.Connections.Value);

            request.AddQueryParameter("customer_id", customerId, true);

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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
            request.AddQueryParameter("connection_id", connectionId, true);

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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
            request.AddQueryParameter("connection_id", connectionId, true);

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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
            request.AddQueryParameter("connection_id", connectionId, true);

            if (!string.IsNullOrWhiteSpace(accountId))
            {
                request.AddQueryParameter("account_id", accountId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/duplicate");

            request.AddJsonBody(new
            {
                data = new
                {
                    customer_id = customerId,
                    transaction_ids = enumerable
                }
            });

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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Transactions.Value}/unduplicate");

            request.AddJsonBody(new
            {
                data = new
                {
                    customer_id = customerId,
                    transaction_ids = enumerable
                }
            });

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

            var request = new RestRequest(SaltEdgeEndpointsV5.Transactions.Value);

            request.AddJsonBody(new
            {
                data = new
                {
                    customer_id = customerId,
                    account_id = accountId,
                    keep_days = keepDays
                }
            });

            var apiResponse = _client.Delete<SimpleResponse<RemoveTransactionResponse>>(request);

            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        public Response<IEnumerable<SeConsent>, SePaging> ConsentsList(string connectionId = default,
            string customerId = default, string fromId = default)
        {
            var request = new RestRequest(SaltEdgeEndpointsV5.Consents.Value);
            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
            }

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                request.AddQueryParameter("from_id", fromId, true);
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

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null," +
                                                   " please visit the documentation : https://docs.saltedge.com/account_information/v5/#consents-show");
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Consents.Value}/{id}");

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
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

            if (string.IsNullOrWhiteSpace(connectionId) && string.IsNullOrWhiteSpace(connectionId))
            {
                throw new InvalidArgumentException("Either connection id or customer id should not be null," +
                                                   " please visit the documentation : https://docs.saltedge.com/account_information/v5/#consents-revoke");
            }

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Consents.Value}/{id}/revoke");

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                request.AddQueryParameter("connection_id", connectionId, true);
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                request.AddQueryParameter("customer_id", customerId, true);
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

            var request = new RestRequest($"{SaltEdgeEndpointsV5.Connections.Value}/learn");

            request.AddJsonBody(new
            {
                data = new
                {
                    customer_id = customerId,
                    transactions = categoryLearns
                }
            });

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
            if (date != null)
            {
                request.AddQueryParameter("date", date.ToString(Config.DateFormat), true);
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

            var request = new RestRequest(SaltEdgeEndpointsV5.Merchants.Value);

            request.AddJsonBody(new
            {
                data = ids
            });

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

            var request = new RestRequest(SaltEdgeEndpointsV5.Merchants.Value);

            request.AddJsonBody(new
            {
                data = new[] {merchantId}
            });

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
            if (!_headers.ContainsKey(""))
            {
                _headers.Add("Content-Type", "application/json");
            }

            if (!_headers.ContainsKey(""))
            {
                _headers.Add("Accept", "application/json");
            }

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