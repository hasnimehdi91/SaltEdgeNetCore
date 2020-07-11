using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.HolderInfo
{
    public class SeHolderInfo
    {
        [JsonProperty("names")]
        public IEnumerable<string> Names { get; set; }

        [JsonProperty("emails")]
        public IEnumerable<string> Emails { get; set; }

        [JsonProperty("phone_numbers")]
        public IEnumerable<string> PhoneNumbers { get; set; }

        [JsonProperty("addresses")]
        public IEnumerable<SeAddress> Addresses { get; set; }

        [JsonProperty("ssn")]
        public string Ssn { get; set; }

         [JsonProperty("cpf")]
        public string Cpf { get; set; }
    }
}