using AIEnterpriseCommandCenter.Domain.Enums;

namespace AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk
{
    public class TicketDto
    {
        public int Id { get; set; }

        public string TicketNumber { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TicketCategory Category { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketStatus Status { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string? AssignedTo { get; set; }

        public string? AISuggestedSolution { get; set; }

        public decimal AIConfidence { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? ResolvedOn { get; set; }
    }
}