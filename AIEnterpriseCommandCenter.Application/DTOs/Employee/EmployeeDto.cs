using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Employee
{
    public  class EmployeeDto
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";


        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string Department { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public DateTime JoiningDate { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
    }
}
