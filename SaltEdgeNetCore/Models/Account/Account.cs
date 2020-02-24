using System;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Account
{
    public class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("nature")]
        public string Nature { get; set; }
        
        [JsonProperty("balance")]
        public decimal? Balance { get; set; }
        
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("extra")]
        public Extra.AccountExtra AccountExtra { get; set; }
        
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}