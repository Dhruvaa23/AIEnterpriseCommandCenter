using AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk;
using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin,IT Admin,Employee,HR Manager")]
    public class ServiceDeskController : Controller
    {
        private readonly IServiceDeskRepository _serviceDeskRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ServiceDeskController(
            IServiceDeskRepository serviceDeskRepository,
            IEmployeeRepository employeeRepository)
        {
            _serviceDeskRepository = serviceDeskRepository;
            _employeeRepository = employeeRepository;
        }

        // ==========================
        // Dashboard
        // ==========================
        public async Task<IActionResult> Index(string? search, int pageNumber = 1)
        {
            const int pageSize = 10;

            ViewBag.Search = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var result = await _serviceDeskRepository.SearchAsync(search);

                return View(result);
            }

            var tickets = await _serviceDeskRepository.GetPagedAsync(pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            ViewBag.TotalPages =
                (int)Math.Ceiling((double)tickets.TotalRecords / pageSize);

            return View(tickets.Items);
        }

        // ==========================
        // Details Popup
        // ==========================
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _serviceDeskRepository.GetByIdAsync(id);

            if (ticket == null)
                return NotFound();

            return PartialView("_TicketModal", ticket);
        }

        // ==========================
        // Create Ticket
        // ==========================
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees =
                await _employeeRepository.GetAllEmployeesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees =
                    await _employeeRepository.GetAllEmployeesAsync();

                return View(dto);
            }

            await _serviceDeskRepository.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Edit
        // ==========================
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _serviceDeskRepository.GetByIdAsync(id);

            if (ticket == null)
                return NotFound();

            var dto = new UpdateTicketDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Category = ticket.Category,
                Priority = ticket.Priority,
                Status = ticket.Status,
                EmployeeId = ticket.EmployeeId,
                AssignedTo = ticket.AssignedTo,
                AISuggestedSolution = ticket.AISuggestedSolution,
                AIConfidence = ticket.AIConfidence
            };

            ViewBag.Employees =
                await _employeeRepository.GetAllEmployeesAsync();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTicketDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees =
                    await _employeeRepository.GetAllEmployeesAsync();

                return View(dto);
            }

            await _serviceDeskRepository.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Delete
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceDeskRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Assign Engineer
        // ==========================
        [HttpPost]
        public async Task<IActionResult> Assign(int id, string assignedTo)
        {
            await _serviceDeskRepository.AssignAsync(id, assignedTo);

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Change Status
        // ==========================
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(
            int id,
            AIEnterpriseCommandCenter.Domain.Enums.TicketStatus status)
        {
            await _serviceDeskRepository.UpdateStatusAsync(id, status);

            return RedirectToAction(nameof(Index));
        }
    }
}