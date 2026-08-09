using System.ComponentModel.DataAnnotations.Schema;

namespace AIEnterpriseCommandCenter.Domain.Entities
{
    public class ProjectEmployee
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}