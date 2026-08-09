using AIEnterpriseCommandCenter.Domain.Enums;

namespace AIEnterpriseCommandCenter.Application.DTOs.Project
{
    public class ProjectDto
    {
      

            public int Id { get; set; }

            public string ProjectCode { get; set; } = string.Empty;

            public string ProjectName { get; set; } = string.Empty;

            public string? Description { get; set; }

            public int ManagerId { get; set; }

            public string ManagerName { get; set; } = string.Empty;

            public ProjectPriority Priority { get; set; }

            public ProjectStatus Status { get; set; }

            public int Progress { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public int TeamMembers { get; set; }
        }
    }

