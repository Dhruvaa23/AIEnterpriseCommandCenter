using AIEnterpriseCommandCenter.Application.Interfaces;

namespace AIEnterpriseCommandCenter.Application.Services;

public class ServiceDeskAITool : IServiceDeskAITool
{
    private readonly IServiceDeskRepository _repository;

    public ServiceDeskAITool(IServiceDeskRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> ExecuteAsync(string prompt)
    {
        prompt = prompt.ToLower();

        var tickets = await _repository.GetAllTicketsAsync();

        //---------------------------------
        // Total Tickets
        //---------------------------------

        if (prompt.Contains("how many tickets") ||
            prompt.Contains("ticket count"))
        {
            return $"There are currently {tickets.Count} tickets.";
        }

        //---------------------------------
        // Open Tickets
        //---------------------------------

        if (prompt.Contains("open tickets"))
        {
            return $"There are {tickets.Count(x => x.Status == Domain.Enums.TicketStatus.Open)} open tickets.";
        }

        //---------------------------------
        // Pending Tickets
        //---------------------------------

        if (prompt.Contains("pending tickets"))
        {
            return $"There are {tickets.Count(x => x.Status == Domain.Enums.TicketStatus.InProgress)} tickets in progress.";
        }

        //---------------------------------
        // Resolved Tickets
        //---------------------------------

        if (prompt.Contains("resolved tickets"))
        {
            return $"There are {tickets.Count(x => x.Status == Domain.Enums.TicketStatus.Resolved)} resolved tickets.";
        }

        //---------------------------------
        // High Priority
        //---------------------------------

        if (prompt.Contains("high priority"))
        {
            var high = tickets
                .Where(x => x.Priority == Domain.Enums.TicketPriority.High)
                .ToList();

            if (!high.Any())
                return "No high priority tickets.";

            return string.Join("\n",
                high.Select(x =>
                    $"{x.TicketNumber} - {x.Title}"));
        }

        //---------------------------------
        // List Tickets
        //---------------------------------

        if (prompt.Contains("list tickets") ||
            prompt.Contains("show tickets"))
        {
            if (!tickets.Any())
                return "No tickets found.";

            return string.Join("\n",
                tickets.Select(x =>
                    $"{x.TicketNumber} - {x.Title} ({x.Status})"));
        }

        //---------------------------------
        // Ticket Number
        //---------------------------------

        var words = prompt.Split(' ');

        var ticketNo = words.FirstOrDefault(x =>
            x.StartsWith("tkt"));

        if (ticketNo != null)
        {
            var ticket =
                await _repository.GetByTicketNumberAsync(ticketNo.ToUpper());

            if (ticket == null)
                return "Ticket not found.";

            return
$"""
Ticket Details

Ticket Number : {ticket.TicketNumber}

Title : {ticket.Title}

Category : {ticket.Category}

Priority : {ticket.Priority}

Status : {ticket.Status}

Employee : {ticket.EmployeeName}


Description : {ticket.Description}
""";
        }

        return null;
    }
}