using System.Collections.Generic;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Attempts;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class Session
    {
        [JsonProperty("attempt")]
        public Attempt Attempt { get; set; }

        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }

        [JsonProperty("return_connection_id")]
        public bool? ReturnConnectionId { get; set; }

        [JsonProperty("provider_modes")]
        public IEnumerable<string> ProviderModes { get; set; }

        [JsonProperty("javascript_callback_type")]
        public string JavascriptCallbackType { get; set; }
        
        [JsonProperty("categorization")]
        public string Categorization { get; set; }

        [JsonProperty("include_fake_providers")]
        public bool? IncludeFakeProviders { get; set; }
        
        [JsonProperty("lost_connection_notify")]
        public bool? LostConnectionNotify { get; set; }

        [JsonProperty("return_error_class")]
        public bool? ReturnErrorClass { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }
    }
}