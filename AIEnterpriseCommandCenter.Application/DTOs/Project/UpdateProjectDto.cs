using AIEnterpriseCommandCenter.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Application.DTOs.Project
{
    public class UpdateProjectDto
    {
        public int Id { get; set; }

       

        [Required]
        public string ProjectName { get; set; } = string.Empty;

        public string? Description { get; set; }

       

        [Required]
        public int ManagerId { get; set; }

      

        public int Progress { get; set; }

        public ProjectPriority Priority { get; set; }

        public ProjectStatus Status { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<int> EmployeeIds { get; set; } = new();
    }
}