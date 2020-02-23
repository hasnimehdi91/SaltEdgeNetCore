using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Error
{
    public class SaltEdgeError
    {
        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("request")]
        public object Request { get; set; }
    }
}