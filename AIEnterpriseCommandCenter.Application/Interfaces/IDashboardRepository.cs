using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIEnterpriseCommandCenter.Application.DTOs.Dashboard;


namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardAsync();

        Task<List<EmployeeChartDto>> GetEmployeeGrowthAsync();

        Task<Dictionary<string, int>> GetAssetDistributionAsync();

        Task<List<TicketStatusChartDto>> GetTicketStatusChartAsync();

        Task<List<ProjectProgressDto>> GetProjectProgressAsync();

        Task<List<ActivityDto>> GetRecentActivitiesAsync();
    }
}
