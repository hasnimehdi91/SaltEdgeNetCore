using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Currencies
{
    public class Currency
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}