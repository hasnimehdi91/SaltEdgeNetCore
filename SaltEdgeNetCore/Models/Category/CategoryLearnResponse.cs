using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Category
{
    public class CategoryLearnResponse
    {
        [JsonProperty("learned")]
        public bool? Learned { get; set; }
    }
}