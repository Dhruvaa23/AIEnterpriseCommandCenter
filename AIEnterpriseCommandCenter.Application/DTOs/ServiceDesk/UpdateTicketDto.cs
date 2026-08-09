using AIEnterpriseCommandCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk
{
    public class UpdateTicketDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public TicketCategory Category { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketStatus Status { get; set; }

        public int EmployeeId { get; set; }

        public string? AssignedTo { get; set; }

        public string? AISuggestedSolution { get; set; }

        public decimal AIConfidence { get; set; }
    }
}