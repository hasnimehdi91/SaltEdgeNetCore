using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Error
{
    public class Request
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}