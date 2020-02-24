using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Consents
{
    public class Consent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        
        [JsonProperty("scopes")]
        public IEnumerable<string> Scopes { get; set; }
        
        [JsonProperty("period_days")]
        public int? PeriodDays { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("collected_by")]
        public string CollectedBy { get; set; }

        [JsonProperty("revoked_at")]
        public DateTime? RevokedAt { get; set; }

        [JsonProperty("revoke_reason")]
        public string RevokeReason { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}