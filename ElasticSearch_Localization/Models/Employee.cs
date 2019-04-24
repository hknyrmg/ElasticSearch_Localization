using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch_Localization.Models
{
    public class Employee
    {
        public int employeeId { get; set; }
        public string Name { get; set; }
        public string jobTitle { get; set; }
        public double resumeScore { get; set; }
        public DateTime dateofStart  { get; set; }

    }
}
