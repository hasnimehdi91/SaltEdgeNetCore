using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class LoginListResponse
    {
        [JsonProperty("data")]
        public IList<Login> Logins { get; set; }
    }
}