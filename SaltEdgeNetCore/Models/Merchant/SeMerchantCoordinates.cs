using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class SeMerchantCoordinates
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}