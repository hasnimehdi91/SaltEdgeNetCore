using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Rates
{
    public class SeRate
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("rate")]
        public decimal? RateValue { get; set; }

        [JsonProperty("issued_on")]
        public string IssuedOn { get; set; }

        [JsonProperty("fail")]
        public bool? Fail { get; set; }
    }
}