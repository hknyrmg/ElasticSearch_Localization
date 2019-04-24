using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElasticSearch_Localization.Models;
using ElasticSearch_Localization.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ElasticSearch_Localization.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            string indexName = $"employees";
            await InsertEmployeesInTurkish(indexName, "tr");
            await InsertEmployeesInEnglish(indexName, "en");

            return View();
        }

        public async Task<IActionResult> Employees(string keyword)
        {
            var requestCultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo culture = requestCultureFeature.RequestCulture.Culture;

            EmployeeSearchResponse employeeSearchResponse = await _employeeService.SearchAsync(keyword, culture.TwoLetterISOLanguageName);

            return View(employeeSearchResponse);
        }

        private async Task InsertEmployeesInTurkish(string indexName, string lang)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
                    employeeId = 1,
                    Name="Ali",
                    jobTitle = "Banka kredi analist",
                    resumeScore=79.2,
                    dateofStart=new DateTime(2010,01,01)
                },
               new Employee
                {
                    employeeId = 2,
                    Name="Veli",
                    jobTitle = "Banka kredi analist",
                    resumeScore=50,
                    dateofStart=new DateTime(2017,01,01)
                },
               new Employee
                {
                    employeeId = 3,
                    Name="Ayşe",
                    jobTitle = "Banka kredi",
                    resumeScore=39.2,
                    dateofStart=new DateTime(2014,01,01)
                },new Employee
                {
                    employeeId = 4,
                    Name="Zeynep",
                    jobTitle = "Banka kredi analist",
                    resumeScore=74.2,
                    dateofStart=new DateTime(2013,01,01)
                },new Employee
                {
                    employeeId = 5,
                    Name="Jessica",
                    jobTitle = "Bankaci",
                    resumeScore=79.2,
                    dateofStart=new DateTime(2019,01,01)
                },new Employee
                {
                    employeeId = 6,
                    Name="Alex",
                    jobTitle = "Banka kredi analist",
                    resumeScore=29.2,
                    dateofStart=new DateTime(2010,01,01)
                },new Employee
                {
                    employeeId = 7,
                    Name="Göksel",
                    jobTitle = "Banka kredi analist",
                    resumeScore=39.2,
                    dateofStart=new DateTime(2018,01,01)
                }
            };
           //İlk çalışmada index oluşturuluyor.
            //await _employeeService.CreateIndexAsync($"{indexName}_{lang}");
            await _employeeService.IndexAsync(employees, lang);
        }

        private async Task InsertEmployeesInEnglish(string indexName, string lang)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
                    employeeId = 1,
                    Name="Ali",
                    jobTitle = "Bank credits analyst",
                    resumeScore=79.2,
                    dateofStart=new DateTime(2010,01,01)
                },
               new Employee
                {
                    employeeId = 2,
                    Name="Veli",
                    jobTitle = "Bank credits analyst",
                    resumeScore=50,
                    dateofStart=new DateTime(2017,01,01)
                },
               new Employee
                {
                    employeeId = 3,
                    Name="Ayşe",
                    jobTitle = "Bank Credit",
                    resumeScore=39.2,
                    dateofStart=new DateTime(2014,01,01)
                },new Employee
                {
                    employeeId = 4,
                    Name="Zeynep",
                    jobTitle = "Bank credits analyst",
                    resumeScore=74.2,
                    dateofStart=new DateTime(2013,01,01)
                },new Employee
                {
                    employeeId = 5,
                    Name="Jessica",
                    jobTitle = "Banker",
                    resumeScore=79.2,
                    dateofStart=new DateTime(2019,01,01)
                },new Employee
                {
                    employeeId = 6,
                    Name="Alex",
                    jobTitle = "Bank credits analyst",
                    resumeScore=29.2,
                    dateofStart=new DateTime(2010,01,01)
                },new Employee
                {
                    employeeId = 7,
                    Name="Göksel",
                    jobTitle = "Bank credits analyst",
                    resumeScore=39.2,
                    dateofStart=new DateTime(2018,01,01)
                }
            };
            //İlk çalışmada index oluşturuluyor.

            //await _employeeService.CreateIndexAsync($"{indexName}_{lang}");
            await _employeeService.IndexAsync(employees, lang);

        }
    }
}
