namespace AIEnterpriseCommandCenter.Application.DTOs.Dashboard;

public class AIInsightDto
{
    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    // Success | Warning | Danger | Info
    public string Severity { get; set; } = "";

    // Bootstrap Icon
    public string Icon { get; set; } = "";
}