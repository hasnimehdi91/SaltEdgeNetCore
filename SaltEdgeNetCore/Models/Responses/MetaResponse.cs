using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Responses
{
    public class Response<T, TV>
    {
        [JsonProperty("data")] 
        public T Data { get; set; }

        [JsonProperty("meta")] 
        public TV Meta { get; set; }
    }
}