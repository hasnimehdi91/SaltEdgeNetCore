using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Extra
{
    public class SeMerchantExtra
    {
        [JsonProperty("building_number")]
        public string BuildingNumber { get; set; }

        [JsonProperty("shop_number")]
        public string ShopNumber { get; set; }

        [JsonProperty("type_of_shop")]
        public string TypeOfShop { get; set; }
    }
}