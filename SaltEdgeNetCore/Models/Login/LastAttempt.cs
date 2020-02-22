using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class LastAttempt
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("finished")]
        public bool? Finished { get; set; }

        [JsonProperty("api_mode")]
        public string ApiMode { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("user_present")]
        public bool? UserPresent { get; set; }

        [JsonProperty("customer_last_logged_at")]
        public object CustomerLastLoggedAt { get; set; }

        [JsonProperty("remote_ip")]
        public string RemoteIp { get; set; }

        [JsonProperty("finished_recent")]
        public bool? FinishedRecent { get; set; }

        [JsonProperty("partial")]
        public bool? Partial { get; set; }

        [JsonProperty("automatic_fetch")]
        public bool? AutomaticFetch { get; set; }

        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }

        [JsonProperty("categorize")]
        public bool? Categorize { get; set; }

        [JsonProperty("identify_merchant")]
        public bool? IdentifyMerchant { get; set; }

        [JsonProperty("custom_fields")]
        public CustomFields CustomFields { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty("exclude_accounts")]
        public IEnumerable<object> ExcludeAccounts { get; set; }

        [JsonProperty("fetch_scopes")]
        public IEnumerable<string> FetchScopes { get; set; }

        [JsonProperty("from_date")]
        public string FromDate { get; set; }

        [JsonProperty("to_date")]
        public string ToDate { get; set; }

        [JsonProperty("interactive")]
        public bool? Interactive { get; set; }

        [JsonProperty("store_credentials")]
        public bool? StoreCredentials { get; set; }

        [JsonProperty("include_natures")]
        public object IncludeNatures { get; set; }

        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }

        [JsonProperty("consent_types")]
        public IEnumerable<string> ConsentTypes { get; set; }

        [JsonProperty("consent_period_days")]
        public object ConsentPeriodDays { get; set; }

        [JsonProperty("consent_given_at")]
        public DateTime? ConsentGivenAt { get; set; }

        [JsonProperty("consent_expires_at")]
        public object ConsentExpiresAt { get; set; }

        [JsonProperty("fail_at")]
        public object FailAt { get; set; }

        [JsonProperty("fail_message")]
        public object FailMessage { get; set; }

        [JsonProperty("fail_error_class")]
        public object FailErrorClass { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("success_at")]
        public DateTime? SuccessAt { get; set; }

        [JsonProperty("last_stage")]
        public LastStage LastStage { get; set; }
    }
}