

using AIEnterpriseCommandCenter.Application.Interfaces;

namespace AIEnterpriseCommandCenter.Application.Services;

public class DashboardAITool : IDashboardAITool
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IServiceDeskRepository _ticketRepository;

    public DashboardAITool(
        IEmployeeRepository employeeRepository,
        IAssetRepository assetRepository,
        IServiceDeskRepository ticketRepository)
    {
        _employeeRepository = employeeRepository;
        _assetRepository = assetRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<string?> ExecuteAsync(string prompt)
    {
        prompt = prompt.ToLower();

        //-------------------------------------------------
        // Dashboard Summary
        //-------------------------------------------------

        if (prompt.Contains("dashboard") ||
            prompt.Contains("summary") ||
            prompt.Contains("company status") ||
            prompt.Contains("statistics") ||
            prompt.Contains("overview"))
        {
            var employees = await _employeeRepository.GetCountAsync();
            var assets = await _assetRepository.GetCountAsync();
            var available = await _assetRepository.GetAvailableCountAsync();
            var assigned = await _assetRepository.GetAssignedCountAsync();
            var tickets = await _ticketRepository.GetOpenCountAsync();
            var highPriority = await _ticketRepository.GetHighPriorityCountAsync();

            return
$"""
📊 Enterprise Dashboard

👨 Total Employees : {employees}

💻 Total Assets : {assets}

✅ Available Assets : {available}

📦 Assigned Assets : {assigned}

🎫 Open Tickets : {tickets}

⚠ High Priority Tickets : {highPriority}

🟢 System Health : Healthy
""";
        }

        //-------------------------------------------------
        // Employees Count
        //-------------------------------------------------

        if (prompt.Contains("total employees"))
        {
            var count = await _employeeRepository.GetCountAsync();

            return $"👨 Total Employees : {count}";
        }

        //-------------------------------------------------
        // Assets Count
        //-------------------------------------------------

        if (prompt.Contains("total assets"))
        {
            var count = await _assetRepository.GetCountAsync();

            return $"💻 Total Assets : {count}";
        }

        //-------------------------------------------------
        // Available Assets
        //-------------------------------------------------

        if (prompt.Contains("available assets"))
        {
            var count = await _assetRepository.GetAvailableCountAsync();

            return $"✅ Available Assets : {count}";
        }

        //-------------------------------------------------
        // Assigned Assets
        //-------------------------------------------------

        if (prompt.Contains("assigned assets"))
        {
            var count = await _assetRepository.GetAssignedCountAsync();

            return $"📦 Assigned Assets : {count}";
        }

        //-------------------------------------------------
        // Open Tickets
        //-------------------------------------------------

        if (prompt.Contains("open tickets"))
        {
            var count = await _ticketRepository.GetOpenCountAsync();

            return $"🎫 Open Tickets : {count}";
        }

        //-------------------------------------------------
        // High Priority Tickets
        //-------------------------------------------------

        if (prompt.Contains("high priority"))
        {
            var count = await _ticketRepository.GetHighPriorityCountAsync();

            return $"⚠ High Priority Tickets : {count}";
        }

        return null;
    }
}