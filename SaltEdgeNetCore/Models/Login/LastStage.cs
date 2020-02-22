using System;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Login
{
    public class LastStage
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("interactive_html")]
        public object InteractiveHtml { get; set; }

        [JsonProperty("interactive_fields_names")]
        public object InteractiveFieldsNames { get; set; }

        [JsonProperty("interactive_fields_options")]
        public object InteractiveFieldsOptions { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}