using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IAIService
    {
        Task<string> AskAsync(List<string> conversation);
    }
}
