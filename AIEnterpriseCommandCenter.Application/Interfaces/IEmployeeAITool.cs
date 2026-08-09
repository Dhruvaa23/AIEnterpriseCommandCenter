using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IEmployeeAITool
    {
        Task<string?> ExecuteAsync(string prompt);

    }
}
