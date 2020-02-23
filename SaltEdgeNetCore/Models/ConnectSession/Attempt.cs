using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class Attempt
    {
        [JsonProperty("fetch_scopes")]
        public IEnumerable<string> FetchScopes { get; set; }
        
        [JsonProperty("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("fetched_accounts_notify")]
        public bool? FetchedAccountsNotify { get; set; }

        [JsonProperty("custom_fields")]
        public object CustomFields { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("include_natures")]
        public IEnumerable<string> IncludeNatures { get; set; }

        [JsonProperty("customer_last_logged_at")]
        public DateTime? CustomerLastLoggedAt { get; set; }

        [JsonProperty("exclude_accounts")]
        public IEnumerable<string> ExcludeAccounts { get; set; }

        [JsonProperty("store_credentials")]
        public bool? StoreCredentials { get; set; }

        [JsonProperty("user_present")]
        public bool? UserPresent { get; set; }

        [JsonProperty("return_to")]
        public string ReturnTo { get; set; }
    }
}