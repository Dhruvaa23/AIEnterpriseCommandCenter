using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.Controllers { 

    [Authorize]
public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IAIInsightsService _aiInsightsService;
        public DashboardController(IDashboardRepository dashboardRepository, IAIInsightsService aIInsightsService)
        {
            _dashboardRepository = dashboardRepository;
            _aiInsightsService = aIInsightsService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = await _dashboardRepository.GetDashboardAsync();
            dashboard.AIInsights =
                await _aiInsightsService.GenerateInsightsAsync();
            return View(dashboard);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeGrowth()
        {
            var data = await _dashboardRepository.GetEmployeeGrowthAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> AssetDistribution()
        {
            var data = await _dashboardRepository.GetAssetDistributionAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> TicketStatusChart()
        {
            var data = await _dashboardRepository.GetTicketStatusChartAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> ProjectProgress()
        {
            var data = await _dashboardRepository.GetProjectProgressAsync();
            return Json(data);
        }

    }

}
