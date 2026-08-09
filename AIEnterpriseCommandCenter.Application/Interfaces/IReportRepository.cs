using AIEnterpriseCommandCenter.Application.DTOs.Asset;
using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using AIEnterpriseCommandCenter.Application.DTOs.Project;
using AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IReportRepository
    {
        // Employee Reports
        Task<List<EmployeeDto>> GetEmployeeReportAsync();

        // Asset Reports
        Task<List<AssetDto>> GetAssetReportAsync();

        // Project Reports
        Task<List<ProjectDto>> GetProjectReportAsync();

        // Service Desk Reports
        Task<List<TicketDto>> GetTicketReportAsync();

        Task<List<EmployeeDto>> ExportEmployeesAsync();

        Task<List<AssetDto>> ExportAssetsAsync();

        Task<List<ProjectDto>> ExportProjectsAsync();

        Task<List<TicketDto>> ExportTicketsAsync();

    }
}