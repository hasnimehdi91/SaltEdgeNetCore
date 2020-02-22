using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Responses
{
    public class SimpleResponse<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}