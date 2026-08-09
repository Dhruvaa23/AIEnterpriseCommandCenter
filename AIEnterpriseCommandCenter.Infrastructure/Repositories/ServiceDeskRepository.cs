using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Domain.Enums;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class ServiceDeskRepository : IServiceDeskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;

        public ServiceDeskRepository(
            ApplicationDbContext context,
            INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task AddAsync(CreateTicketDto dto)
        {
            var ticket = new ServiceTicket
            {
                TicketNumber = $"TCK-{DateTime.Now.Ticks.ToString()[^4..]}",
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Priority = dto.Priority,
                EmployeeId = dto.EmployeeId,
                Status = TicketStatus.Open,
                CreatedOn = DateTime.Now,
                AIConfidence = dto.AIConfidence,
                AISuggestedSolution = dto.AISuggestedSolution,
            };

            _context.ServiceTickets.Add(ticket);

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "New Service Ticket",
                $"{ticket.TicketNumber} - {ticket.Title} has been created.",
                "Service Desk");
        }

        public async Task UpdateAsync(UpdateTicketDto dto)
        {
            var ticket = await _context.ServiceTickets.FindAsync(dto.Id);

            if (ticket == null)
                return;

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.Category = dto.Category;
            ticket.Priority = dto.Priority;
            ticket.Status = dto.Status;
            ticket.EmployeeId = dto.EmployeeId;
            ticket.AssignedTo = dto.AssignedTo;
            ticket.AISuggestedSolution = dto.AISuggestedSolution;
            ticket.AIConfidence = dto.AIConfidence;
            ticket.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
    "Ticket Updated",
    $"{ticket.TicketNumber} was updated.",
    "Service Desk");
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.ServiceTickets.FindAsync(id);

            if (ticket == null)
                return;

            var ticketNo = ticket.TicketNumber;

            _context.ServiceTickets.Remove(ticket);

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Ticket Deleted",
                $"{ticketNo} has been deleted.",
                "Service Desk");
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ServiceTickets.AnyAsync(x => x.Id == id);
        }

        public async Task<TicketDto?> GetByIdAsync(int id)
        {
            return await _context.ServiceTickets
                .Include(x => x.Employee)
                .Where(x => x.Id == id)
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    TicketNumber = x.TicketNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Category = x.Category,
                    Priority = x.Priority,
                    Status = x.Status,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee != null ? x.Employee.FullName : "",
                    AssignedTo = x.AssignedTo,
                    AISuggestedSolution = x.AISuggestedSolution,
                    AIConfidence = x.AIConfidence,
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn,
                    ResolvedOn = x.ResolvedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<TicketDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.ServiceTickets.Include(x => x.Employee);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    TicketNumber = x.TicketNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Category = x.Category,
                    Priority = x.Priority,
                    Status = x.Status,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.Employee != null ? x.Employee.FullName : "",
                    AssignedTo = x.AssignedTo,
                    AISuggestedSolution = x.AISuggestedSolution,
                    AIConfidence = x.AIConfidence,
                    CreatedOn = x.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<TicketDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total
            };
        }

        public async Task<IEnumerable<TicketDto>> SearchAsync(string searchTerm)
        {
            return await _context.ServiceTickets
                .Include(x => x.Employee)
                .Where(x =>
                    x.TicketNumber.Contains(searchTerm) ||
                    x.Title.Contains(searchTerm))
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    TicketNumber = x.TicketNumber,
                    Title = x.Title,
                    Status = x.Status,
                    Priority = x.Priority,
                    EmployeeName = x.Employee != null ? x.Employee.FullName : ""
                })
                .ToListAsync();
        }

        public async Task AssignAsync(int ticketId, string assignedTo)
        {
            var ticket = await _context.ServiceTickets.FindAsync(ticketId);

            if (ticket == null)
                return;

            ticket.AssignedTo = assignedTo;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Ticket Assigned",
                $"{ticket.TicketNumber} assigned to {assignedTo}.",
                "Service Desk");
        }

        public async Task UpdateStatusAsync(int ticketId, TicketStatus status)
        {
            var ticket = await _context.ServiceTickets.FindAsync(ticketId);

            if (ticket == null)
                return;

            ticket.Status = status;

            if (status == TicketStatus.Resolved)
                ticket.ResolvedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Ticket Status Updated",
                $"{ticket.TicketNumber} status changed to {status}.",
                "Service Desk");
        }

        public async Task UpdateAISolutionAsync(int ticketId, string solution, decimal confidence)
        {
            var ticket = await _context.ServiceTickets.FindAsync(ticketId);

            if (ticket == null)
                return;

            ticket.AISuggestedSolution = solution;
            ticket.AIConfidence = confidence;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "AI Solution Generated",
                $"AI generated a solution for {ticket.TicketNumber}.",
                "AI");
        }

        public async Task<List<TicketDto>> ExportAsync()
        {
            return await _context.ServiceTickets
                .Include(x => x.Employee)
                .Select(x => new TicketDto
                {
                    TicketNumber = x.TicketNumber,
                    Title = x.Title,
                    Category = x.Category,
                    Priority = x.Priority,
                    Status = x.Status,
                    EmployeeName = x.Employee != null ? x.Employee.FullName : ""
                })
                .ToListAsync();
        }

        public async Task<ServiceDeskDashboardDto> GetDashboardAsync()
        {
            return new ServiceDeskDashboardDto
            {
                TotalTickets = await _context.ServiceTickets.CountAsync(),
                OpenTickets = await _context.ServiceTickets.CountAsync(x => x.Status == TicketStatus.Open),
                InProgressTickets = await _context.ServiceTickets.CountAsync(x => x.Status == TicketStatus.InProgress),
                ResolvedTickets = await _context.ServiceTickets.CountAsync(x => x.Status == TicketStatus.Resolved),
                ClosedTickets = await _context.ServiceTickets.CountAsync(x => x.Status == TicketStatus.Closed),
                CriticalTickets = await _context.ServiceTickets.CountAsync(x => x.Priority == TicketPriority.Critical)
            };
        }
        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            return await _context.ServiceTickets
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Title = t.Title,
                    Description = t.Description,
                    Category = t.Category,
                    Priority = t.Priority,
                    Status = t.Status,
                    EmployeeId = t.EmployeeId,
                    EmployeeName = t.Employee.FirstName + " " + t.Employee.LastName,
                    CreatedOn = t.CreatedOn,
                    AISuggestedSolution = t.AISuggestedSolution,
                    AIConfidence = t.AIConfidence,
                    UpdatedOn = t.UpdatedOn,
                    ResolvedOn = t.ResolvedOn
                })
                .ToListAsync();
        }
        public async Task<TicketDto?> GetByTicketNumberAsync(string ticketNumber)
        {
            return await _context.ServiceTickets
                .Where(t => t.TicketNumber == ticketNumber)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Title = t.Title,
                    Category = t.Category,
                    Priority = t.Priority,
                    Status = t.Status,
                    EmployeeId = t.EmployeeId,
                    EmployeeName = t.Employee.FirstName + " " + t.Employee.LastName,
                    Description = t.Description,
                    AISuggestedSolution = t.AISuggestedSolution,
                    AIConfidence = t.AIConfidence,
                    CreatedOn = t.CreatedOn,
                    UpdatedOn = t.UpdatedOn,
                    ResolvedOn = t.ResolvedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.ServiceTickets.CountAsync();
        }

        public async Task<int> GetOpenCountAsync()
        {
            return await _context.ServiceTickets
                .CountAsync(x => x.Status == TicketStatus.Open);
        }

        public async Task<int> GetHighPriorityCountAsync()
        {
            return await _context.ServiceTickets
                .CountAsync(x => x.Priority == TicketPriority.High);
        }
    }

}