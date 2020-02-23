using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Responses
{
    public class Paging
    {
        [JsonProperty("next_id")]
        public object NextId { get; set; }

        [JsonProperty("next_page")]
        public object NextPage { get; set; }
    }
}