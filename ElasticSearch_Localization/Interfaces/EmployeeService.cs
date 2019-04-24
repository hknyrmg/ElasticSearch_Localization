using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch_Localization.Models;
using Nest;

namespace ElasticSearch_Localization.Interfaces
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ElasticClient _elasticClient;

        public EmployeeService(ConnectionSettings connectionSettings)
        {
            _elasticClient = new ElasticClient(connectionSettings);
        }

        public async Task CreateIndexAsync(string indexName)
        {
            var createIndexDescriptor = new CreateIndexDescriptor(indexName.ToLowerInvariant())
                                         .Mappings(m => m.Map<Employee>(p => p.AutoMap()));

            await _elasticClient.CreateIndexAsync(createIndexDescriptor);
        }

        public async Task<bool> IndexAsync(List<Employee> employees, string langCode)
        {
            string indexName = $"employees_{langCode}";

            IBulkResponse response = await _elasticClient.IndexManyAsync(employees, indexName);

            return response.IsValid;
        }


        public async Task<EmployeeSearchResponse> SearchAsync(string keyword, string langCode)
        {
            EmployeeSearchResponse employeeSearchResponse = new EmployeeSearchResponse();
            string indexName = $"employees_{langCode}";

            ISearchResponse<Employee> searchResponse = await _elasticClient.SearchAsync<Employee>(x => x
                .Index(indexName)
                .Query(q =>
                            q.MultiMatch(mp => mp
                                        .Query(keyword)
                                        .Fields(f => f.Fields(f1 => f1.jobTitle,
                                        f2=> f2.Name
                                        ))) && q.DateRange(r => r
           .Field(f => f.dateofStart)
           .LessThan(DateTime.Today.AddYears(-5))
        )
                ));

            if (searchResponse.IsValid && searchResponse.Documents != null)
            {
                employeeSearchResponse.Total = (int)searchResponse.Total;
                employeeSearchResponse.Employees = searchResponse.Documents;
            }

            return employeeSearchResponse;
        }
    }
}

