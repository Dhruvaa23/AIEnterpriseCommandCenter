using AIEnterpriseCommandCenter.Application.DTOs.Dashboard;

namespace AIEnterpriseCommandCenter.Application.Services
{
    public interface IAIInsightsService
    {
        Task<List<AIInsightDto>> GenerateInsightsAsync();
    }
}
