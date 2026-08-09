using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.AI
{
    public class TicketAnalysisDto
    {
        public string Title { get; set; } = "";

        public string Category { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public string SuggestedSolution { get; set; } = string.Empty;

        public int Confidence { get; set; }
    }
}
