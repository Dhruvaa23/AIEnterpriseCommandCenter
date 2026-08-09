using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin,IT Admin,HR Manager")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IDashboardRepository _dashboardRepository;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDashboardRepository dashboardRepository)
        {
            _employeeRepository = employeeRepository;
            _dashboardRepository = dashboardRepository;
        }


        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            ViewBag.Dashboard = await _dashboardRepository.GetDashboardAsync();

            const int pageSize = 5;

            PagedResult<EmployeeDto> result;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                result = await _employeeRepository.SearchAsync(searchTerm, page, pageSize);
            }
            else
            {
                result = await _employeeRepository.GetAllAsync(page, pageSize);
            }

            ViewBag.SearchTerm = searchTerm;

            return View(result);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            if (await _employeeRepository.EmployeeCodeExistsAsync(dto.EmployeeCode))
            {
                ModelState.AddModelError("EmployeeCode", "Employee Code already exists.");
                return View(dto);
            }

            if (await _employeeRepository.EmailExistsAsync(dto.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(dto);
            }

            await _employeeRepository.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var dto = new UpdateEmployeeDto
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Department = employee.Department,
                Designation = employee.Designation,
                JoiningDate = employee.JoiningDate,
                Salary = employee.Salary,
                IsActive = employee.IsActive
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _employeeRepository.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Details
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }
    }
}
