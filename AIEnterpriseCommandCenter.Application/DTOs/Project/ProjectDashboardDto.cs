namespace AIEnterpriseCommandCenter.Application.DTOs.Project
{
    public class ProjectDashboardDto
    {
        public int TotalProjects { get; set; }

        public int ActiveProjects { get; set; }

        public int PlanningProjects { get; set; }

        public int CompletedProjects { get; set; }

        public int OnHoldProjects { get; set; }

        public int CancelledProjects { get; set; }

        public decimal TotalBudget { get; set; }
    }
}