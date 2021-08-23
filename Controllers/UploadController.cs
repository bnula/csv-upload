using AutoMapper;
using CsvHelper;
using CsvImport.Models;
using CsvImport.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport.Controllers
{
    public class UploadController : Controller
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;

        public UploadController(
            IEmployeeRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file, [FromServices] IWebHostEnvironment hostingEnv)
        {
            string fileName = $"{hostingEnv.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                await fileStream.FlushAsync();
            }

            var empDtos = this.GetEmployees(file.FileName);
            var emps = _mapper.Map<List<Employee>>(empDtos);
            var success = await this.CreateEmployees(emps);

            if (!success)
            {
                ModelState.AddModelError("x", "Something went Wrong");
                return View();
            }
            
            return View();
        }

        private List<EmployeeDto> GetEmployees(string fileName)
        {
            var emps = new List<EmployeeDto>();

            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var emp = csv.GetRecord<EmployeeDto>();
                    emps.Add(emp);
                }
            }

            return emps;
        }

        private async Task<bool> CreateEmployees(List<Employee> employees)
        {
            foreach (var emp in employees)
            {
                var success = await _repo.CreateAsync(emp);
                if (!success)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
