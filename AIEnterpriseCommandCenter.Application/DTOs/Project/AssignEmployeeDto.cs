using System.ComponentModel.DataAnnotations;

namespace AIEnterpriseCommandCenter.Application.DTOs.Project;

public class AssignEmployeeDto
{
    public int ProjectId { get; set; }

    [Display(Name = "Project")]
    public string ProjectName { get; set; } = "";

    public List<int> SelectedEmployeeIds { get; set; } = new();
}