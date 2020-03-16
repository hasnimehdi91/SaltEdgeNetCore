using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Provider
{
    public class SeRequiredField
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
        public bool? Optional { get; set; }

        [JsonProperty("extra")]
        public Extra.SeAccountExtra AccountExtra { get; set; }

        [JsonProperty("position")]
        public int? Position { get; set; }
    }
}