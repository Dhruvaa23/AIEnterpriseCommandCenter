using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.Project;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IProjectRepository
    {
        // Dashboard
        Task<PagedResult<ProjectDto>> GetPagedAsync(int pageNumber, int pageSize);

        Task<IEnumerable<ProjectDto>> SearchAsync(string search);

        Task<ProjectDto?> GetByIdAsync(int id);

        // CRUD
        Task AddAsync(CreateProjectDto dto);

        Task UpdateAsync(UpdateProjectDto dto);

        Task DeleteAsync(int id);

        // Validation
        Task<bool> ExistsAsync(int id);

        // Export
        Task<List<ProjectDto>> ExportAsync();

        Task AssignEmployeesAsync(int projectId, List<int> employeeIds);
    }
}