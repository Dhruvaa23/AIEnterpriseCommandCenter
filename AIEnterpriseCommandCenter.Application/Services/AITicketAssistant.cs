

using AIEnterpriseCommandCenter.Application.DTOs.AI;
using AIEnterpriseCommandCenter.Application.Interfaces;

namespace AIEnterpriseCommandCenter.Application.Services;

public class AITicketAssistant : IAITicketAssistant
{
    private readonly IAIService _aiService;

    public AITicketAssistant(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<TicketAnalysisDto> AnalyzeAsync(string issue)
    {
        // First check Rule Engine
        var ruleResult = TicketRuleEngine.Analyze(issue);

        if (ruleResult != null)
            return ruleResult;

        // Unknown issue -> Ask Llama

        var history = new List<string>
{
$$"""
You are an Enterprise IT Ticket Classifier.

Return ONLY valid JSON.

Schema:

```json
{
  "Title": "",
  "Category": "",
  "Priority": "",
  "SuggestedSolution": "",
  "Confidence": 0
}
```

Allowed Category:

Hardware
Software
Network
Account
Security
Printer
Email
VPN
Other

Allowed Priority:

Low
Medium
High
Critical

Rules

- Do not return markdown.
- Do not explain.
- Return JSON only.
- Confidence must be between 0 and 100.

Issue:

{issue}
"""
};

        try
        {
            var response = await _aiService.AskAsync(history);

            var parsed = System.Text.Json.JsonSerializer.Deserialize<TicketAnalysisDto>(
                response,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (parsed == null)
            {
                return new TicketAnalysisDto
                {
                    Title = "IT Support Request",
                    Category = "Other",
                    Priority = "Medium",
                    SuggestedSolution = "Please assign this ticket to the IT support team.",
                    Confidence = 50
                };
            }

            // Validate Category
            string[] categories =
            {
        "Hardware",
        "Software",
        "Network",
        "Account",
        "Security",
        "Printer",
        "Email",
        "VPN",
        "Other"
    };

            if (!categories.Contains(parsed.Category))
                parsed.Category = "Other";

            // Validate Priority
            string[] priorities =
            {
        "Low",
        "Medium",
        "High",
        "Critical"
    };

            if (!priorities.Contains(parsed.Priority))
                parsed.Priority = "Medium";

            parsed.Confidence = Math.Clamp(parsed.Confidence, 0, 100);

            return parsed;
        }
        catch
        {
            return new TicketAnalysisDto
            {
                Title = "Unknown IT Issue",
                Category = "Other",
                Priority = "Medium",
                SuggestedSolution = "AI service is currently unavailable.",
                Confidence = 0
            };
        }
    }

    
}