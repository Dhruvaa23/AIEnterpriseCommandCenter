using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AIEnterpriseCommandCenter.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;

        public EmployeeRepository(
            ApplicationDbContext context,
            INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task<PagedResult<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Employees.AsQueryable();

            var totalRecords = await query.CountAsync();

            var employees = await query
                .OrderBy(e => e.EmployeeCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Department = e.Department,
                    Designation = e.Designation,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return new PagedResult<EmployeeDto>
            {
                Items = employees,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Department = e.Department,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                EmployeeCode = dto.EmployeeCode,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                Designation = dto.Designation,
                JoiningDate = dto.JoiningDate,
                Salary = dto.Salary,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            // Create Notification
            await _notificationRepository.CreateAsync(
                "New Employee Added",
                $"{employee.FirstName} {employee.LastName} joined {employee.Department} Department.",
                "Employee");
        }

        public async Task UpdateAsync(UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(dto.Id);

            if (employee == null)
                return;

            employee.EmployeeCode = dto.EmployeeCode;
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.Department = dto.Department;
            employee.Designation = dto.Designation;
            employee.JoiningDate = dto.JoiningDate;
            employee.Salary = dto.Salary;
            employee.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            await _notificationRepository.CreateAsync(
    "Employee Updated",
    $"{employee.FirstName} {employee.LastName}'s profile was updated.",
    "Employee");
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return;
            var employeeName = $"{employee.FirstName} {employee.LastName}";
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
    "Employee Removed",
    $"{employeeName} has been removed from the company.",
    "Employee");
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> EmployeeCodeExistsAsync(string employeeCode)
        {
            return await _context.Employees
                .AnyAsync(e => e.EmployeeCode == employeeCode);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email);
        }

        public async Task<PagedResult<EmployeeDto>> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                query = query.Where(e =>
                    e.EmployeeCode.ToLower().Contains(searchTerm) ||
                    e.FirstName.ToLower().Contains(searchTerm) ||
                    e.LastName.ToLower().Contains(searchTerm) ||
                    e.Email.ToLower().Contains(searchTerm) ||
                    e.Department.ToLower().Contains(searchTerm));
            }

            var totalRecords = await query.CountAsync();

            var employees = await query
                .OrderBy(e => e.EmployeeCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Department = e.Department,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .ToListAsync();

            return new PagedResult<EmployeeDto>
            {
                Items = employees,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<IEnumerable<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Employees
                .OrderBy(e => e.EmployeeCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Department = e.Department,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Employees.CountAsync();
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .OrderBy(e => e.FirstName)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmployeeCode = e.EmployeeCode,
                    Department = e.Department,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto?> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _context.Employees
                .Where(e => e.EmployeeCode == employeeCode)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    EmployeeCode = e.EmployeeCode,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Department = e.Department,
                    Designation = e.Designation,
                    JoiningDate = e.JoiningDate,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}
