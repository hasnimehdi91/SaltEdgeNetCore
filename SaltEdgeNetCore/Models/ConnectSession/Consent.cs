using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class Consent
    {
        [JsonProperty("scopes")]
        public IEnumerable<string> Scopes { get; set; }

        [JsonProperty("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("period_days")]
        public int? PeriodDays { get; set; }
    }
}