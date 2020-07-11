using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Country
{
    public class SeCountry
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("refresh_start_time")]
        public int? RefreshStartTime { get; set; }
    }
}