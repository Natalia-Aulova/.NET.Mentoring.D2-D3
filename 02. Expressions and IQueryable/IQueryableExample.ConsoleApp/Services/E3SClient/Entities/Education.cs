using Newtonsoft.Json;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    public class Education
    {
        [JsonProperty(PropertyName = "institution")]
        public string Institution { get; set; }

        [JsonProperty(PropertyName = "degree")]
        public string Degree { get; set; }

        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }

        [JsonProperty(PropertyName = "faculty")]
        public string Faculty { get; set; }

        [JsonProperty(PropertyName = "graduationYear")]
        public string GraduationYear { get; set; }
    }
}
