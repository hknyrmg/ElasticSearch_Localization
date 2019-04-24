using ElasticSearch_Localization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch_Localization.Interfaces
{
   public  interface IEmployeeService
    {
        Task CreateIndexAsync(string indexName);
        Task<bool> IndexAsync(List<Employee> employees, string langCode);
        Task<EmployeeSearchResponse> SearchAsync(string keyword, string langCode);
    }
}
