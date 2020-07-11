using System;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Extra;

namespace SaltEdgeNetCore.Models.Transaction
{
    public class SaltEdgeTransaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("made_on")]
        public DateTime? MadeOn { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("duplicated")]
        public bool? Duplicated { get; set; }

        [JsonProperty("extra")]
        public SeTransactionExtra Extra { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}