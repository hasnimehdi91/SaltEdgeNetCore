using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class LoginResponse
    {
        [JsonProperty("data")]
        public Login Login { get; set; }
    }
}