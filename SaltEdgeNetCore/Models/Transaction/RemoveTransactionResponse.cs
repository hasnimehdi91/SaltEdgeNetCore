using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Transaction
{
    public class RemoveTransactionResponse
    {
        [JsonProperty("cleanup_started")]
        public bool? CleanupStarted { get; set; }
    }
}