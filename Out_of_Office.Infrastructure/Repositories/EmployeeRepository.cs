﻿using Microsoft.EntityFrameworkCore;
using Out_of_Office.Domain.Entities;
using Out_of_Office.Domain.Interfaces;
using Out_of_Office.Infrastructure.Presistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Out_of_OfficeDbContext _dbContext;

        public EmployeeRepository(Out_of_OfficeDbContext dbContext)
        {
            _dbContext=dbContext;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employee.ToListAsync();
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            _dbContext.Employee.Add(employee);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int Id)
        {
            return await _dbContext.Employee.FindAsync(Id);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employee.Update(employee);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IList<Employee>> GetProjectManagersAsync()
        {
            return await _dbContext.Employee
                                 .Where(e => e.Position == "Project Manager")
                                 .ToListAsync();
        }
    }
}
