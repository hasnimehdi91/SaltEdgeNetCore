using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class Login
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        [JsonProperty("provider_code")]
        public string ProviderCode { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("holder_info")]
        public HolderInfo HolderInfo { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("next_refresh_possible_at")]
        public DateTime? NextRefreshPossibleAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("categorization")]
        public string Categorization { get; set; }

        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }

        [JsonProperty("store_credentials")]
        public bool? StoreCredentials { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("last_success_at")]
        public DateTime? LastSuccessAt { get; set; }

        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }

        [JsonProperty("consent_types")]
        public IList<string> ConsentTypes { get; set; }

        [JsonProperty("consent_period_days")]
        public object ConsentPeriodDays { get; set; }

        [JsonProperty("consent_given_at")]
        public DateTime? ConsentGivenAt { get; set; }

        [JsonProperty("consent_expires_at")]
        public object ConsentExpiresAt { get; set; }

        [JsonProperty("last_attempt")]
        public LastAttempt LastAttempt { get; set; }
    }
}