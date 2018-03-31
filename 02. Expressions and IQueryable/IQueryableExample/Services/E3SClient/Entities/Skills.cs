using Newtonsoft.Json;

namespace IQueryableExample.Services.E3SClient.Entities
{
    public class Skills
    {
        [JsonProperty(PropertyName = "nativespeaker")]
        public string NativeSpeaker { get; set; }

        [JsonProperty(PropertyName = "expert")]
        public string Expert { get; set; }

        [JsonProperty(PropertyName = "advanced")]
        public string Advanced { get; set; }

        [JsonProperty(PropertyName = "intermediate")]
        public string Intermediate { get; set; }

        [JsonProperty(PropertyName = "novice")]
        public string Novice { get; set; }

        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "os")]
        public string Os { get; set; }

        [JsonProperty(PropertyName = "db")]
        public string Db { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "industry")]
        public string Industry { get; set; }

        [JsonProperty(PropertyName = "proglang")]
        public string Proglang { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "other")]
        public string Other { get; set; }

        [JsonProperty(PropertyName = "primary")]
        public string Primary { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}
