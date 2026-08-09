using AIEnterpriseCommandCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Domain.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ProjectCode { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ProjectName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(150)]
        public string ClientName { get; set; } = string.Empty;

        // Project Manager
        public int ManagerId { get; set; }

        public Employee? Manager { get; set; }

        public decimal Budget { get; set; }

        public int Progress { get; set; }

        public ProjectPriority Priority { get; set; }

        public ProjectStatus Status { get; set; }

        public ProjectHealth Health { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }

        // Employees assigned to this project
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
            = new List<ProjectEmployee>();
    }
}