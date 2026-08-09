using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Dashboard
{
    public  class DashboardDto
    {
        public int TotalEmployees { get; set; }
        public int TotalAssets { get; set; }
        public int OpenTickets { get; set; }
        public int AIAlerts { get; set; }

        public int TotalProjects { get; set; }

        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }

        public int TotalDepartments { get; set; }

        public List<ActivityDto> RecentActivities { get; set; } = new();

        public List<AIInsightDto> AIInsights { get; set; } = new();
    }
}
