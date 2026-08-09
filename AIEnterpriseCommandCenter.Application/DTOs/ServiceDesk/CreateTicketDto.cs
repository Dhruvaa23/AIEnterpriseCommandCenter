using AIEnterpriseCommandCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk
{
    public class CreateTicketDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TicketCategory Category { get; set; }

        [Required]
        public TicketPriority Priority { get; set; }

        [Required]
        public string AISuggestedSolution { get; set; }

        public int AIConfidence { get; set; }

        [Required]
        public int EmployeeId { get; set; }
    }
}