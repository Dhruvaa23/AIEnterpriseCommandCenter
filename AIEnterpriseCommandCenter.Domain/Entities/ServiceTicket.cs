
using AIEnterpriseCommandCenter.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIEnterpriseCommandCenter.Domain.Entities
{
    public class ServiceTicket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TicketNumber { get; set; } = string.Empty;

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
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        // Employee who raised the ticket
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        // Assigned IT Engineer
        [StringLength(100)]
        public string? AssignedTo { get; set; }

        // AI Generated Suggestion
        [StringLength(1000)]
        public string? AISuggestedSolution { get; set; }

        // AI Confidence (0-100)
        public decimal AIConfidence { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public DateTime? ResolvedOn { get; set; }
    }
}
