using AIEnterpriseCommandCenter.Application.DTOs.Project;
using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin,IT Admin")]

    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ProjectController(
            IProjectRepository projectRepository,
            IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }

        // ==========================
        // Project List
        // ==========================
        public async Task<IActionResult> Index(string? search, int pageNumber = 1)
        {
            const int pageSize = 6;

            ViewBag.Search = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var result = await _projectRepository.SearchAsync(search);
                return View(result);
            }

            var resultPaged = await _projectRepository.GetPagedAsync(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages =
                (int)Math.Ceiling((double)resultPaged.TotalRecords / pageSize);

            return View(resultPaged.Items);
        }

        // ==========================
        // Create
        // ==========================
        public async Task<IActionResult> Create()
        {
            ViewBag.Managers = new SelectList(
                await _employeeRepository.GetAllEmployeesAsync(),
                "Id",
                "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Managers = new SelectList(
                    await _employeeRepository.GetAllEmployeesAsync(),
                    "Id",
                    "FullName");

                return View(dto);
            }

            await _projectRepository.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        
        // Project Details
        
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return NotFound();

            return PartialView("_ProjectModal", project);
        }

        // ==========================
        // Edit
        // ==========================

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return NotFound();

            var dto = new UpdateProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                ManagerId = project.ManagerId,
                Priority = project.Priority,
                Status = project.Status,
                Progress = project.Progress,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };

            ViewBag.Managers = new SelectList(
                await _employeeRepository.GetAllEmployeesAsync(),
                "Id",
                "FullName",
                dto.ManagerId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Managers = new SelectList(
                    await _employeeRepository.GetAllEmployeesAsync(),
                    "Id",
                    "FullName",
                    dto.ManagerId);

                return View(dto);
            }

            await _projectRepository.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _projectRepository.DeleteAsync(id);

            TempData["Success"] = "Project deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignEmployees(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return NotFound();

            var dto = new AssignEmployeeDto
            {
                ProjectId = project.Id,
                ProjectName = project.ProjectName
            };

            ViewBag.Employees = await _employeeRepository.GetAllEmployeesAsync();

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AssignEmployees(AssignEmployeeDto dto)
        {
            await _projectRepository.AssignEmployeesAsync(
                dto.ProjectId,
                dto.SelectedEmployeeIds);

            TempData["Success"] = "Employees assigned successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}