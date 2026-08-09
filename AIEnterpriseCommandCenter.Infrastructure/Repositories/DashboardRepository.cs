using AIEnterpriseCommandCenter.Application.DTOs.Dashboard;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Enums;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            return new DashboardDto
            {
                TotalEmployees = await _context.Employees.CountAsync(),

                ActiveEmployees = await _context.Employees
                    .CountAsync(x => x.IsActive),

                InactiveEmployees = await _context.Employees
                    .CountAsync(x => !x.IsActive),

                TotalDepartments = await _context.Employees
                    .Select(x => x.Department)
                    .Distinct()
                    .CountAsync(),

                // These modules are not created yet
                TotalAssets = await _context.Assets.CountAsync(),

                OpenTickets = await _context.ServiceTickets.CountAsync(x => x.Status == TicketStatus.Open),


                TotalProjects = await _context.Projects.CountAsync(),

                RecentActivities = await GetRecentActivitiesAsync()
            };
        }

        public async Task<List<EmployeeChartDto>> GetEmployeeGrowthAsync()
        {
            var result = await _context.Employees
                .GroupBy(x => new
                {
                    x.JoiningDate.Year,
                    x.JoiningDate.Month
                 })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new EmployeeChartDto
                {
                     Month = new DateTime(g.Key.Year, g.Key.Month, 1)
                        .ToString("MMM yyyy"),

                 TotalEmployees = g.Count()
                 })
                .ToListAsync(); 
            return result;
        }

        public async Task<Dictionary<string, int>> GetAssetDistributionAsync()
        {
            return await _context.Assets
                .GroupBy(a => a.Category)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count());
        }

        public async Task<List<TicketStatusChartDto>> GetTicketStatusChartAsync()
        {
            return await _context.ServiceTickets
                .GroupBy(x => x.Status)
                .Select(g => new TicketStatusChartDto
                {
                    Status = g.Key.ToString(),
                    Total = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<ProjectProgressDto>> GetProjectProgressAsync()
        {
            return await _context.Projects
                .OrderByDescending(x => x.Progress)
                .Select(x => new ProjectProgressDto
                {
                    ProjectName = x.ProjectName,
                    Progress = x.Progress
                })
                .ToListAsync();
        }

        public async Task<List<ActivityDto>> GetRecentActivitiesAsync()
        {
            var employees = await _context.Employees
                .OrderByDescending(x => x.JoiningDate)
                .Take(5)
                .Select(x => new ActivityDto
                {
                    Title = "Employee Added",
                    Description = $"{x.FullName} joined {x.Department}",
                    Icon = "bi bi-person-plus-fill",
                    Color = "bg-success",
                    Date = x.JoiningDate
                })
                .ToListAsync();

            var assets = await _context.Assets
                .OrderByDescending(x => x.PurchaseDate)
                .Take(5)
                .Select(x => new ActivityDto
                {
                    Title = "Asset Added",
                    Description = $"{x.AssetName} ({x.AssetCode}) added",
                    Icon = "bi bi-laptop",
                    Color = "bg-primary",
                    Date = x.PurchaseDate
                })
                .ToListAsync();

            var tickets = await _context.ServiceTickets
                .OrderByDescending(x => x.CreatedOn)
                .Take(5)
                .Select(x => new ActivityDto
                {
                    Title = "Ticket Created",
                    Description = x.Title,
                    Icon = "bi bi-headset",
                    Color = "bg-warning",
                    Date = x.CreatedOn
                })
                .ToListAsync();

            var projects = await _context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(5)
                .Select(x => new ActivityDto
                {
                    Title = "Project Created",
                    Description = x.ProjectName,
                    Icon = "bi bi-kanban-fill",
                    Color = "bg-info",
                    Date = x.StartDate
                })
                .ToListAsync();

            return employees
                .Concat(assets)
                .Concat(tickets)
                .Concat(projects)
                .OrderByDescending(x => x.Date)
                .Take(5)
                .ToList();
        }
    }
}