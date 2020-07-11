using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Transaction
{
    public class DuplicatedResponse
    {
        [JsonProperty("duplicated")]
        public bool? Duplicated { get; set; }
    }
}