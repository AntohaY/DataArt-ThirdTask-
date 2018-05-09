using Newtonsoft.Json;
using System.Collections.Generic;

namespace dotnetSchoolTask3
{
    public class BankClient
    {
        // [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public List<Operation> Operations { get; set; }
    }
}
