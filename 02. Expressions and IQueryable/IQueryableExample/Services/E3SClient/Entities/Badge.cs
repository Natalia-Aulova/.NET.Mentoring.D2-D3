using Newtonsoft.Json;

namespace IQueryableExample.Services.E3SClient.Entities
{
    public class Badge
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}
