using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Extra;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class Address
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("transliterated_city")]
        public string TransliteratedCity { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("transliterated_street")]
        public string TransliteratedStreet { get; set; }

        [JsonProperty("post_code")]
        public string PostCode { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("extra")]
        public MerchantExtra Extra { get; set; }
    }
}