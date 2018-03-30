using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    public class Recognition
    {
        [JsonProperty]
        public string nomination { get; set; }

        [JsonProperty]
        public string description { get; set; }

        [JsonProperty]
        public string recognitiondate { get; set; }

        [JsonProperty]
        public string points { get; set; }
    }
}
