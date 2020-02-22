using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Error
{
    public class Error
    {
        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("documentation_url")]
        public string DocumentationUrl { get; set; }
    }
}