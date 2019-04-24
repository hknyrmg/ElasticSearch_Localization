using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch_Localization.Models
{
    public class EmployeeSearchResponse
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
