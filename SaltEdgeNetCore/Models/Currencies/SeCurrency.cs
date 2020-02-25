using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Currencies
{
    public class SeCurrency
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}