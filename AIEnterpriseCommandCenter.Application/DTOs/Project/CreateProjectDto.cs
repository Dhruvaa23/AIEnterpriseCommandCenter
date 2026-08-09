using AIEnterpriseCommandCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Application.DTOs.Project
{
    public class CreateProjectDto
    {
        [Required]
        [StringLength(150)]
        public string ProjectName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int ManagerId { get; set; }

        [Required]
        public ProjectPriority Priority { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        // Selected Employees
        public List<int> EmployeeIds { get; set; } = new();
    }
}