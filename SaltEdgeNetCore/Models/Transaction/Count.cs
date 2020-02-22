using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Transaction
{
    public class Count
    {
        [JsonProperty("posted")]
        public int Posted { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }
    }
}