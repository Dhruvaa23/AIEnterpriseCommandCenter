using AIEnterpriseCommandCenter.Application.DTOs.Dashboard;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Application.Services;
using System.Text.Json;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class AIInsightsService : IAIInsightsService
    {
        private readonly IAIService _aiService;

        private readonly IDashboardRepository _dashboardRepository;

        public AIInsightsService(
            IAIService aiService,
            IDashboardRepository dashboardRepository)
        {
            _aiService = aiService;
            _dashboardRepository = dashboardRepository;
        }

        public async Task<List<AIInsightDto>> GenerateInsightsAsync()
        {
            var dashboard = await _dashboardRepository.GetDashboardAsync();

            var prompt = $@"
You are an Enterprise AI Business Analyst for an AI Enterprise Command Center.

Analyze the following live dashboard data and provide intelligent business recommendations.

=========================
ENTERPRISE DASHBOARD
=========================

👥 Employees
- Total Employees: {dashboard.TotalEmployees}

💻 Assets
- Total Assets: {dashboard.TotalAssets}

🎫 Service Desk
- Open Tickets: {dashboard.OpenTickets}

📁 Projects
- Total Projects: {dashboard.TotalProjects}

=========================
YOUR TASK
=========================

Generate EXACTLY FOUR executive insights.

Each insight must have one lines.

Line 1:
A short business title.

Line 2:
A professional recommendation using the dashboard numbers.

Rules:

• Mention actual numbers whenever possible.
• Think like a CIO or IT Manager.
• Give useful recommendations.
• Keep each recommendation under 10 words.
• Avoid generic statements.
• Do not use markdown.
• Do not use JSON.
• Do not use bullets.
• Do not use numbering.
• Return ONLY the insights.

Good Examples:

Employee Workforce
The organization currently has 120 employees. Plan recruitment according to upcoming projects.

Asset Utilization
90 company assets require regular maintenance and lifecycle monitoring.

Service Desk
12 tickets remain open. Prioritize high-impact issues to improve response time.

Project Portfolio
8 active projects require continuous progress monitoring to avoid schedule delays.
";

            var response = await _aiService.AskAsync(
                new List<string>
                 {
                    prompt
                 });

            response = response
                .Replace("```", "")
                .Replace("**", "")
                .Replace("#", "")
                .Replace("*", "")
                .Replace("-", "")
                .Replace("•", "")
                .Trim();

            try
            {
                var lines = response
    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(x => x.Trim())
    .ToList();

                var insights = new List<AIInsightDto>();

                for (int i = 0; i < lines.Count - 1; i += 2)
                {
                    var title = lines[i];
                    var description = lines[i + 1];

                    var insight = new AIInsightDto
                    {
                        Title = title,
                        Description = description
                    };

                    if (title.Contains("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        insight.Icon = "bi bi-graph-up-arrow";
                        insight.Severity = "Success";
                    }
                    else if (title.Contains("Ticket", StringComparison.OrdinalIgnoreCase))
                    {
                        insight.Icon = "bi bi-headset";
                        insight.Severity = "Warning";
                    }
                    else if (title.Contains("Asset", StringComparison.OrdinalIgnoreCase))
                    {
                        insight.Icon = "bi bi-laptop";
                        insight.Severity = "Info";
                    }
                    else if (title.Contains("Project", StringComparison.OrdinalIgnoreCase))
                    {
                        insight.Icon = "bi bi-kanban";
                        insight.Severity = "Danger";
                    }
                    else
                    {
                        insight.Icon = "bi bi-stars";
                        insight.Severity = "Info";
                    }

                    insights.Add(insight);
                }

                return insights;
            }
            catch
            {
                return new List<AIInsightDto>
    {
        new()
        {
            Title="Employee Workforce",
            Description=$"Company currently has {dashboard.TotalEmployees} employees.",
            Severity="Success",
            Icon="bi bi-people"
        },

        new()
        {
            Title="Asset Management",
            Description=$"{dashboard.TotalAssets} assets require monitoring.",
            Severity="Info",
            Icon="bi bi-laptop"
        },

        new()
        {
            Title="Service Desk",
            Description=$"{dashboard.OpenTickets} tickets are currently open.",
            Severity="Warning",
            Icon="bi bi-headset"
        },

        new()
        {
            Title="Project Portfolio",
            Description=$"{dashboard.TotalProjects} projects are under execution.",
            Severity="Danger",
            Icon="bi bi-kanban"
        }
    };
            }
        } 
       
     }
 }
