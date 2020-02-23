using System;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Attempts;

namespace SaltEdgeNetCore.Models.Connections
{
    public class Connection
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }

        [JsonProperty("last_consent_id")]
        public string LastConsentId { get; set; }

        [JsonProperty("last_attempt")]
        public Attempt LastAttempt { get; set; }

        [JsonProperty("last_success_at")]
        public DateTime? LastSuccessAt { get; set; }

        [JsonProperty("next_refresh_possible_at")]
        public DateTime? NextRefreshPossibleAt { get; set; }

        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        [JsonProperty("provider_code")]
        public string ProviderCode { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("store_credentials")]
        public bool? StoreCredentials { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}