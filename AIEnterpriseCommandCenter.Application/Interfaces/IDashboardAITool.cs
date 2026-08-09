using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IDashboardAITool
    {
        Task<string?> ExecuteAsync(string prompt);
    }
}
