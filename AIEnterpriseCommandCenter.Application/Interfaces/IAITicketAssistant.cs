using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnterpriseCommandCenter.Application.DTOs.AI;


namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IAITicketAssistant
    {
        Task<TicketAnalysisDto> AnalyzeAsync(string issue);

    }
}
