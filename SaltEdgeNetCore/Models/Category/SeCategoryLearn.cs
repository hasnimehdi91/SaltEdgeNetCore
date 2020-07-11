using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Category
{
    public class SeCategoryLearn
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("category_code")]
        public string CategoryCode { get; set; }

        [JsonProperty("immediate")]
        public bool Immediate { get; set; }
    }
}