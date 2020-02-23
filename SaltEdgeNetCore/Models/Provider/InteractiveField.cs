using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Account;

namespace SaltEdgeNetCore.Models.Provider
{
    public class InteractiveField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("english_name")]
        public string EnglishName { get; set; }

        [JsonProperty("localized_name")]
        public string LocalizedName { get; set; }

        [JsonProperty("nature")]
        public string Nature { get; set; }

        [JsonProperty("optional")]
        public bool Optional { get; set; }

        [JsonProperty("extra")]
        public Extra Extra { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }
}