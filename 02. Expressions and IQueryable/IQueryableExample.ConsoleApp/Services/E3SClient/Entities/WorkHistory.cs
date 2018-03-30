using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    public class WorkHistory
    {
        [JsonProperty]
        public string terms { get; set; }

        [JsonProperty]
        public string company { get; set; }

        [JsonProperty]
        public string companyurl { get; set; }

        [JsonProperty]
        public string position { get; set; }

        [JsonProperty]
        public string role { get; set; }

        [JsonProperty]
        public string project { get; set; }

        [JsonProperty]
        public string participation { get; set; }

        [JsonProperty]
        public string team { get; set; }

        [JsonProperty]
        public string database { get; set; }

        [JsonProperty]
        public string tools { get; set; }

        [JsonProperty]
        public string technologies { get; set; }

        [JsonProperty]
        public string startdate { get; set; }

        [JsonProperty]
        public string enddate { get; set; }

        [JsonProperty]
        public bool isepam { get; set; }

        [JsonProperty]
        public string epamproject { get; set; }
    }
}
