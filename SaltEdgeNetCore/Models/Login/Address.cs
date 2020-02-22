using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class Address
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("post_code")]
        public string PostCode { get; set; }
    }
}