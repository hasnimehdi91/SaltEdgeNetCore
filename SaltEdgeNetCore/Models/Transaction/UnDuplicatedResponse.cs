using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Transaction
{
    public class UnDuplicatedResponse
    {
        [JsonProperty("unduplicated")]
        public bool? UnDuplicated { get; set; }
    }
}