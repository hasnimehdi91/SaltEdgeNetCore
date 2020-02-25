using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class Merchant
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("names")]
        public IEnumerable<SeMerchantName> Names { get; set; }

        [JsonProperty("contact")]
        public IEnumerable<SeMerchantContact> Contact { get; set; }

        [JsonProperty("address")]
        public SeMerchantAddress Address { get; set; }
    }
}