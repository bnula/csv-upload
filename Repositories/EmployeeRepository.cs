using CsvImport.Data;
using CsvImport.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvImport.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _db;

        public EmployeeRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Employee entity)
        {
            await _db.Employees.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Employee entity)
        {
            _db.Employees.Remove(entity);
            return await SaveAsync();
        }

        public async Task<IEnumerable<Employee>> FindAllAsync()
        {
            var items = await _db.Employees.ToListAsync();
            return items;
        }

        public async Task<Employee> FindByIdAsync(int id)
        {
            var item = await _db.Employees
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync();
            return item;
        }

        public async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Employee entity)
        {
            _db.Employees.Update(entity);
            return await SaveAsync();
        }
    }
}
