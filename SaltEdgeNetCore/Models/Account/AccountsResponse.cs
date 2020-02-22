using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Account
{
    public class AccountsResponse
    {
        [JsonProperty("data")]
        public IEnumerable<SaltEdgeBankAccount> Data { get; set; }
    }
}