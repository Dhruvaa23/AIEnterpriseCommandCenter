using AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk;
using AIEnterpriseCommandCenter.Application.Common;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IServiceDeskRepository
    {
        // Dashboard
        Task<ServiceDeskDashboardDto> GetDashboardAsync();

        // Ticket List with Pagination
        Task<PagedResult<TicketDto>> GetPagedAsync(int pageNumber, int pageSize);

        // Search
        Task<IEnumerable<TicketDto>> SearchAsync(string searchTerm);

        // Details
        Task<TicketDto?> GetByIdAsync(int id);

        // CRUD
        Task AddAsync(CreateTicketDto dto);

        Task UpdateAsync(UpdateTicketDto dto);

        Task DeleteAsync(int id);

        // Ticket Assignment
        Task AssignAsync(int ticketId, string assignedTo);

        // Ticket Status
        Task UpdateStatusAsync(int ticketId, Domain.Enums.TicketStatus status);

        // AI Suggestion
        Task UpdateAISolutionAsync(int ticketId, string solution, decimal confidence);

        // Export
        Task<List<TicketDto>> ExportAsync();

        // Validation
        Task<bool> ExistsAsync(int id);

        Task<TicketDto?> GetByTicketNumberAsync(string ticketNumber);

        Task<List<TicketDto>> GetAllTicketsAsync();

        Task<int> GetCountAsync();

        Task<int> GetOpenCountAsync();

        Task<int> GetHighPriorityCountAsync();
    }
}