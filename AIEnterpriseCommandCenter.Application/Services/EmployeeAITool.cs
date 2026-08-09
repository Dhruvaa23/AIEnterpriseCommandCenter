

using AIEnterpriseCommandCenter.Application.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AIEnterpriseCommandCenter.Application.Services;

public class EmployeeAITool : IEmployeeAITool
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeAITool(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<string?> ExecuteAsync(string prompt)
    {
        prompt = prompt.ToLower().Trim();

        var employees = await _employeeRepository.GetAllEmployeesAsync();

        //-----------------------------------------
        // Employee Count
        //-----------------------------------------

        if (prompt.Contains("how many employees") ||
            prompt.Contains("employee count") ||
            prompt.Contains("total employees"))
        {
            return $"There are currently {employees.Count} employees in the company.";
        }

        //-----------------------------------------
        // List Employees
        //-----------------------------------------

        if (prompt.Contains("list employees") ||
            prompt.Contains("show employees") ||
            prompt.Contains("employee list") ||
            prompt.Contains("all employees"))
        {
            if (!employees.Any())
                return "No employees found.";

            StringBuilder sb = new();

            sb.AppendLine($"Employees ({employees.Count})");
            sb.AppendLine("--------------------------------");

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeCode} | {e.FirstName} {e.LastName} | {e.Department}");
            }

            return sb.ToString();
        }

        //-----------------------------------------
        // Active Employees
        //-----------------------------------------

        if (prompt.Contains("active employees"))
        {
            var active = employees.Where(x => x.IsActive).ToList();

            if (!active.Any())
                return "No active employees found.";

            return string.Join(Environment.NewLine,
                active.Select(x =>
                    $"{x.EmployeeCode} - {x.FirstName} {x.LastName}"));
        }

        //-----------------------------------------
        // Inactive Employees
        //-----------------------------------------

        if (prompt.Contains("inactive employees"))
        {
            var inactive = employees.Where(x => !x.IsActive).ToList();

            if (!inactive.Any())
                return "No inactive employees found.";

            return string.Join(Environment.NewLine,
                inactive.Select(x =>
                    $"{x.EmployeeCode} - {x.FirstName} {x.LastName}"));
        }

        //-----------------------------------------
        // Department Search
        //-----------------------------------------

        string[] departments =
        {
            "it department",
            "hr",
            "finance",
            "sales",
            "marketing",
            "admin"
        };

        foreach (var department in departments)
        {
            if (prompt.Contains(department))
            {
                var deptEmployees = employees
                    .Where(x => x.Department.Equals(
                        department,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!deptEmployees.Any())
                    return $"No employees found in {department.ToUpper()} department.";

                StringBuilder sb = new();

                sb.AppendLine($"{department.ToUpper()} Department");
                sb.AppendLine("--------------------------------");

                foreach (var emp in deptEmployees)
                {
                    sb.AppendLine($"{emp.EmployeeCode} - {emp.FirstName} {emp.LastName}");
                }

                return sb.ToString();
            }
        }

        //-----------------------------------------
        // Highest Salary Employee
        //-----------------------------------------

        if (prompt.Contains("highest salary") ||
            prompt.Contains("top salary") ||
            prompt.Contains("highest paid"))
        {
            var employee = employees
                .OrderByDescending(x => x.Salary)
                .FirstOrDefault();

            if (employee == null)
                return "No employees found.";

            return
$"""
Highest Paid Employee

Employee Code : {employee.EmployeeCode}

Name          : {employee.FirstName} {employee.LastName}

Department    : {employee.Department}

Designation   : {employee.Designation}

Salary        : ₹{employee.Salary:N2}
""";
        }

        //-----------------------------------------
        // Employee Code Search
        //-----------------------------------------

        var match = Regex.Match(prompt, @"emp\d+", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var code = match.Value.ToUpper();

            var employee =
                await _employeeRepository.GetByEmployeeCodeAsync(code);

            if (employee == null)
                return $"Employee {code} not found.";

            return
$"""
Employee Information
────────────────────────

Employee Code : {employee.EmployeeCode}

Name          : {employee.FirstName} {employee.LastName}

Department    : {employee.Department}

Designation   : {employee.Designation}

Email         : {employee.Email}

Status        : {(employee.IsActive ? "Active" : "Inactive")}

Joining Date  : {employee.JoiningDate:dd-MMM-yyyy}

Salary        : ₹{employee.Salary:N2}
""";
        }

        //-----------------------------------------
        // Not Handled
        //-----------------------------------------

        return null;
    }
}