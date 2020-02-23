using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Attempts
{
    public class Attempt
    {
        [JsonProperty("api_mode")]
        public string ApiMode { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

        [JsonProperty("automatic_fetch")]
        public bool? AutomaticFetch { get; set; }

        [JsonProperty("user_present")]
        public bool? UserPresent { get; set; }

        [JsonProperty("categorization")]
        public string Categorization { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("customer_last_logged_at")]
        public DateTime? CustomerLastLoggedAt { get; set; }

        [JsonProperty("custom_fields")]
        public object CustomFields { get; set; }

        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("remote_ip")]
        public string RemoteIp { get; set; }

        [JsonProperty("exclude_accounts")]
        public IEnumerable<string> ExcludeAccounts { get; set; }

        [JsonProperty("fail_at")]
        public DateTime? FailAt { get; set; }

        [JsonProperty("fail_error_class")]
        public string FailErrorClass { get; set; }

        [JsonProperty("fail_message")]
        public string FailMessage { get; set; }

        [JsonProperty("fetch_scopes")]
        public IEnumerable<string> FetchScopes { get; set; }

        [JsonProperty("finished")]
        public bool? Finished { get; set; }

        [JsonProperty("finished_recent")]
        public bool? FinishedRecent { get; set; }

        [JsonProperty("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("interactive")]
        public bool? Interactive { get; set; }

        [JsonProperty("partial")]
        public bool? Partial { get; set; }

        [JsonProperty("store_credentials")]
        public bool? StoreCredentials { get; set; }

        [JsonProperty("success_at")]
        public DateTime? SuccessAt { get; set; }

        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }

        [JsonProperty("include_natures")]
        public IEnumerable<string> IncludeNatures { get; set; }

        [JsonProperty("last_stage")]
        public Stage Stage { get; set; }

        [JsonProperty("stages")]
        public IEnumerable<Stage> Type { get; set; }
    }
}