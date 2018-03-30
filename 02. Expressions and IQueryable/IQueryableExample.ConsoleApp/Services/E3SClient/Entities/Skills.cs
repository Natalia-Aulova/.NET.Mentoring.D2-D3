using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    public class Skills
    {
        [JsonProperty]
        public string nativespeaker { get; set; }

        [JsonProperty]
        public string expert { get; set; }

        [JsonProperty]
        public string advanced { get; set; }

        [JsonProperty]
        public string intermediate { get; set; }

        [JsonProperty]
        public string novice { get; set; }

        [JsonProperty]
        public string position { get; set; }

        [JsonProperty]
        public string os { get; set; }

        [JsonProperty]
        public string db { get; set; }

        [JsonProperty]
        public string platform { get; set; }

        [JsonProperty]
        public string industry { get; set; }

        [JsonProperty]
        public string proglang { get; set; }

        [JsonProperty]
        public string language { get; set; }

        [JsonProperty]
        public string other { get; set; }

        [JsonProperty]
        public string primary { get; set; }

        [JsonProperty]
        public string category { get; set; }
    }
}
