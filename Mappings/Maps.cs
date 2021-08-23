using AutoMapper;
using CsvImport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport.Mappings
{
    public class Maps: Profile
    {
        public Maps()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
