using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Extra;

namespace SaltEdgeNetCore.Models.Provider
{
    public class SeProvider
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("automatic_fetch")]
        public bool? AutomaticFetch { get; set; }
        
        [JsonProperty("customer_notified_on_sign_in")]
        public bool? CustomerNotifiedOnSignIn { get; set; }
        
        [JsonProperty("interactive")]
        public bool? Interactive { get; set; }
        
        [JsonProperty("identification_mode")]
        public string IdentificationMode { get; set; }
        
        [JsonProperty("instruction")]
        public string Instruction { get; set; }
        
        [JsonProperty("home_url")]
        public string HomeUrl { get; set; }

        [JsonProperty("login_url")]
        public string LoginUrl { get; set; }

        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        
        [JsonProperty("refresh_timeout")]
        public int? RefreshTimeout { get; set; }
        
        [JsonProperty("holder_info")]
        public IEnumerable<string> HolderInfo { get; set; }
        
        [JsonProperty("max_consent_days")]
        public int? MaxConsentDays { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        
        [JsonProperty("max_interactive_delay")]
        public int? MaxInteractiveDelay { get; set; }
        
        [JsonProperty("optional_interactivity")]
        public bool? OptionalInteractivity { get; set; }
        
        [JsonProperty("regulated")]
        public bool? Regulated { get; set; }
        
        [JsonProperty("max_fetch_interval")]
        public int? MaxFetchInterval { get; set; }
        
        [JsonProperty("supported_fetch_scopes")]
        public IEnumerable<string> SupportedFetchScopes { get; set; }
        
        [JsonProperty("supported_account_extra_fields")]
        public IEnumerable<SeAccountExtra> SupportedAccountExtraFields { get; set; }

        [JsonProperty("supported_transaction_extra_fields")]
        public IEnumerable<SeTransactionExtra> SupportedTransactionExtraFields { get; set; }

        [JsonProperty("supported_account_natures")]
        public IEnumerable<string> SupportedAccountNatures { get; set; }

        [JsonProperty("supported_account_types")]
        public IEnumerable<string> SupportedAccountTypes { get; set; }
        
        [JsonProperty("payment_templates")]
        public IEnumerable<object> PaymentTemplates { get; set; }

        [JsonProperty("required_fields")]
        public IEnumerable<SeRequiredField> RequiredFields { get; set; }

        [JsonProperty("interactive_fields")]
        public IEnumerable<SeInteractiveField> InteractiveFields { get; set; }

        [JsonProperty("forum_url")]
        public string ForumUrl { get; set; }
    }
}