using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public  interface IEmployeeRepository
    {
        Task<PagedResult<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize);

        Task<EmployeeDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateEmployeeDto employeeDto);

        Task UpdateAsync(UpdateEmployeeDto employeeDto);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> EmployeeCodeExistsAsync(string employeeCode);

        Task<bool> EmailExistsAsync(string email);

        Task<PagedResult<EmployeeDto>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task<IEnumerable<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize);

        Task<int> GetCountAsync();

        Task<List<EmployeeDto>> GetAllEmployeesAsync();

        Task<EmployeeDto?> GetByEmployeeCodeAsync(string employeeCode);

    }
}
