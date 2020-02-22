using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class HolderInfo
    {
        [JsonProperty("names")]
        public IEnumerable<string> Names { get; set; }

        [JsonProperty("emails")]
        public IEnumerable<string> Emails { get; set; }

        [JsonProperty("phone_numbers")]
        public IEnumerable<string> PhoneNumbers { get; set; }

        [JsonProperty("addresses")]
        public IEnumerable<Address> Addresses { get; set; }
    }
}