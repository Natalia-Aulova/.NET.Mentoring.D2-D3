using Newtonsoft.Json;
using System.Collections.Generic;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    [E3SMetaType("meta:people-suite:people-api:com.epam.e3s.app.people.api.data.EmployeeEntity")]
    public class EmployeeEntity : E3SEntity
    {
        [JsonProperty]
        double entityBoost { get; set; }

        [JsonProperty]
        double documentBoost { get; set; }

        [JsonProperty]
        List<string> phone;

        [JsonProperty]
        Skills skill { get; set; }

        [JsonProperty]
        List<string> firstname { get; set; }

        [JsonProperty]
        List<string> lastname { get; set; }

        [JsonProperty]
        List<string> fullname { get; set; }

        [JsonProperty]
        List<string> country { get; set; }

        [JsonProperty]
        List<string> city { get; set; }

        [JsonProperty]
        List<string> email { get; set; }

        [JsonProperty]
        List<string> skype { get; set; }

        [JsonProperty]
        List<string> social { get; set; }

        [JsonProperty]
        List<string> attachment { get; set; }

        [JsonProperty]
        public string manager { get; set; }

        [JsonProperty]
        public string superior { get; set; }

        [JsonProperty]
        public string startworkdate { get; set; }

        [JsonProperty]
        public string project { get; set; }

        [JsonProperty]
        public string projectall { get; set; }

        [JsonProperty]
        List<string> trainer { get; set; }

        [JsonProperty]
        List<string> kb { get; set; }

        [JsonProperty]
        public string certificate { get; set; }

        [JsonProperty]
        public string unit { get; set; }

        [JsonProperty]
        public string office { get; set; }

        [JsonProperty]
        public string room { get; set; }

        [JsonProperty]
        public string status { get; set; }

        [JsonProperty]
        public string car { get; set; }

        [JsonProperty]
        public string birthday { get; set; }

        [JsonProperty]
        public List<WorkHistory> workhistory { get; set; }


        [JsonProperty]
        List<string> jobfunction { get; set; }

        [JsonProperty]
        List<Recognition> recognition { get; set; }

        [JsonProperty]
        List<string> badge { get; set; }

        [JsonProperty]
        public string dismissal { get; set; }

        [JsonProperty]
        public string endProbationDate { get; set; }

        [JsonProperty]
        public string endworkdate { get; set; }

        [JsonProperty]
        public string errupdatedate { get; set; }

        [JsonProperty]
        public string edulevel { get; set; }

        [JsonProperty]
        public string eduschool { get; set; }

        [JsonProperty]
        public string edufield { get; set; }

        [JsonProperty]
        public string eduendyear { get; set; }

        [JsonProperty]
        public string workstation { get; set; }

        [JsonProperty]
        public string nativename { get; set; }

        [JsonProperty]
        public string governmentalid { get; set; }

        [JsonProperty]
        public double billable { get; set; }

        [JsonProperty]
        public double nonbillable { get; set; }
    }
}
