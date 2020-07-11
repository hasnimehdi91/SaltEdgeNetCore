using System.Collections.Generic;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Consents;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class CreateSession: SeSession
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        
        [JsonProperty("consent")]
        public SeConsent Consent { get; set; }

        [JsonProperty("allowed_countries")]
        public IEnumerable<string> AllowedCountries { get; set; }

        [JsonProperty("provider_code")]
        public string ProviderCode { get; set; }

        [JsonProperty("disable_provider_search")]
        public bool? DisableProviderSearch { get; set; }

        [JsonProperty("show_consent_confirmation")]
        public bool? ShowConsentConfirmation { get; set; }

        [JsonProperty("credentials_strategy")]
        public string CredentialsStrategy { get; set; }
    }
}