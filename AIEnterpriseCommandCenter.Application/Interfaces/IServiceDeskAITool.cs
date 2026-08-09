using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IServiceDeskAITool
    {
        Task<string?> ExecuteAsync(string prompt);
    }
}
