using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class Merchant
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("names")]
        public IEnumerable<Name> Names { get; set; }

        [JsonProperty("contact")]
        public IEnumerable<Contact> Contact { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
    }
}