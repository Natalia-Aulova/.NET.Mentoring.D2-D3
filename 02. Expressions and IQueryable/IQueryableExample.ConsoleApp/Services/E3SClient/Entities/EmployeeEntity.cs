using Newtonsoft.Json;
using System.Collections.Generic;

namespace IQueryableExample.ConsoleApp.Services.E3SClient.Entities
{
    [E3SMetaType("meta:people-suite:people-api:com.epam.e3s.app.people.api.data.pluggable.EmployeeEntity")]
    public class EmployeeEntity : E3SEntity
    {
        [JsonProperty(PropertyName = "documentBoost")]
        public double DocumentBoost { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public List<string> Phones { get; set; }
        
        [JsonProperty(PropertyName = "skill")]
        public Skills Skills { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public List<string> FullNames { get; set; }

        [JsonProperty(PropertyName = "nativeName")]
        public string NativeName { get; set; }

        [JsonProperty(PropertyName = "country")]
        public List<string> Countries { get; set; }

        [JsonProperty(PropertyName = "city")]
        public List<string> Cities { get; set; }

        [JsonProperty(PropertyName = "email")]
        public List<string> Emails { get; set; }
        
        [JsonProperty(PropertyName = "manager")]
        public string Manager { get; set; }

        [JsonProperty(PropertyName = "superior")]
        public string Superior { get; set; }

        [JsonProperty(PropertyName = "shortStartWorkDate")]
        public string StartWorkDate { get; set; }

        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }

        [JsonProperty(PropertyName = "projectAll")]
        public string ProjectAll { get; set; }

        [JsonProperty(PropertyName = "trainer")]
        public List<string> Trainers { get; set; }
        
        [JsonProperty(PropertyName = "office")]
        public string Office { get; set; }

        [JsonProperty(PropertyName = "room")]
        public string Room { get; set; }

        [JsonProperty(PropertyName = "employmentStatus")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "car")]
        public string Car { get; set; }
        
        [JsonProperty(PropertyName = "badge")]
        public List<Badge> Badges { get; set; }
        
        [JsonProperty(PropertyName = "education")]
        public List<Education> Education { get; set; }

        [JsonProperty(PropertyName = "workStation")]
        public string WorkStation { get; set; }

        [JsonProperty(PropertyName = "workPlace")]
        public string WorkPlace { get; set; }

        [JsonProperty(PropertyName = "billable")]
        public double Billable { get; set; }

        [JsonProperty(PropertyName = "nonBillable")]
        public double NonBillable { get; set; }
    }
}
