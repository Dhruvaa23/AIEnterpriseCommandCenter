using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnterpriseCommandCenter.Application.Interfaces;

namespace AIEnterpriseCommandCenter.Application.Services
{
    public class AIToolsService : IAIToolsService
    {
        private readonly IEmployeeAITool _employeeTool;
        private readonly IAssetAITool _assetTool;
        private readonly IServiceDeskAITool _ticketAI;
        private readonly IAITicketAssistant _ticketAssistant;
        private readonly IDashboardAITool _dashboardTool;

        public AIToolsService(IEmployeeAITool employeeTool, IAssetAITool assetTool, IServiceDeskAITool ticketAI, IAITicketAssistant ticketAssistant, IDashboardAITool dashboardTool)
        {
            _employeeTool = employeeTool;
            _assetTool = assetTool;
            _ticketAI = ticketAI;
            _ticketAssistant = ticketAssistant;
            _dashboardTool = dashboardTool;
        }

        public async Task<string?> ExecuteAsync(string prompt)
        {
            var result = await _employeeTool.ExecuteAsync(prompt);

            if (result != null)
                return result;

            result = await _assetTool.ExecuteAsync(prompt);

            if (result != null)
                return result;


            result = await _dashboardTool.ExecuteAsync(prompt);

            if (result != null)
                return result;

            string[] ticketKeywords =
{
        "error",
        "issue",
        "problem",
        "not working",
        "blue screen",
        "bsod",
        "printer",
        "password",
        "login",
        "wifi",
        "internet",
        "vpn",
        "outlook",
        "email",
        "laptop",
        "computer",
        "keyboard",
        "mouse",
        "monitor",
        "software",
        "application",
        "crash",
        "freeze",
        "slow",
        "hang",
        "restart"
    };

            if (ticketKeywords.Any(k =>
                prompt.Contains(k, StringComparison.OrdinalIgnoreCase)))
            {
                var ticket = await _ticketAssistant.AnalyzeAsync(prompt);

                
                    return
        $"""
🎫 Ticket Analysis

Title:
{ticket.Title}

Category:
{ticket.Category}

Priority:
{ticket.Priority}

Suggested Solution:
{ticket.SuggestedSolution}

Confidence:
{ticket.Confidence}%
""";
                }
            

            result = await _ticketAI.ExecuteAsync(prompt);

            if (result != null)
                return result;


            return null;
        }
    }
    }
