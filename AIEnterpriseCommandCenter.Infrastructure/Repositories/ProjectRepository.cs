using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.Project;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;

        public ProjectRepository(
            ApplicationDbContext context,
            INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task AddAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                ProjectCode = $"PRJ-{DateTime.Now.Ticks.ToString()[^4..]}",
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                ManagerId = dto.ManagerId,
                Priority = dto.Priority,
                Status = dto.Status,
                Progress = 0,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            foreach (var employeeId in dto.EmployeeIds)
            {
                _context.ProjectEmployees.Add(new ProjectEmployee
                {
                    ProjectId = project.Id,
                    EmployeeId = employeeId
                });
            }

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "New Project Created",
                $"{project.ProjectName} has been created.",
                "Project");
        }

        public async Task UpdateAsync(UpdateProjectDto dto)
        {
            var project = await _context.Projects.FindAsync(dto.Id);

            if (project == null)
                return;

            project.ProjectName = dto.ProjectName;
            project.Description = dto.Description;
            project.ManagerId = dto.ManagerId;
            project.Priority = dto.Priority;
            project.Status = dto.Status;
            project.Progress = dto.Progress;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
    "Project Updated",
    $"{project.ProjectName} was updated.",
    "Project");
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return;

            var projectName = project.ProjectName;

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Project Deleted",
                $"{projectName} has been deleted.",
                "Project");
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(x => x.Id == id);
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees)
                .Where(x => x.Id == id)
                .Select(x => new ProjectDto
                {
                    Id = x.Id,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    Description = x.Description,
                    ManagerId = x.ManagerId,
                    ManagerName = x.Manager.FullName,
                    Priority = x.Priority,
                    Status = x.Status,
                    Progress = x.Progress,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TeamMembers = x.ProjectEmployees.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<ProjectDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProjectDto
                {
                    Id = x.Id,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    ManagerId = x.ManagerId,
                    ManagerName = x.Manager.FullName,
                    Progress = x.Progress,
                    Priority = x.Priority,
                    Status = x.Status,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TeamMembers = x.ProjectEmployees.Count
                })
                .ToListAsync();

            return new PagedResult<ProjectDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total
            };
        }

        public async Task<IEnumerable<ProjectDto>> SearchAsync(string search)
        {
            return await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees)
                .Where(x =>
                    x.ProjectName.Contains(search) ||
                    x.ProjectCode.Contains(search))
                .Select(x => new ProjectDto
                {
                    Id = x.Id,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    ManagerName = x.Manager.FullName,
                    Status = x.Status,
                    Progress = x.Progress,
                    TeamMembers = x.ProjectEmployees.Count
                })
                .ToListAsync();
        }

        public async Task<List<ProjectDto>> ExportAsync()
        {
            return await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees)
                .Select(x => new ProjectDto
                {
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    ManagerName = x.Manager.FullName,
                    Status = x.Status,
                    Progress = x.Progress,
                    TeamMembers = x.ProjectEmployees.Count
                })
                .ToListAsync();
        }

        public async Task AssignEmployeesAsync(int projectId, List<int> employeeIds)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project == null)
                return;

            var oldEmployees = _context.ProjectEmployees
                .Where(x => x.ProjectId == projectId);

            _context.ProjectEmployees.RemoveRange(oldEmployees);

            foreach (var employeeId in employeeIds)
            {
                _context.ProjectEmployees.Add(new ProjectEmployee
                {
                    ProjectId = projectId,
                    EmployeeId = employeeId
                });
            }

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Project Team Updated",
                $"Team members updated for {project.ProjectName}.",
                "Project");
        }
    }
}