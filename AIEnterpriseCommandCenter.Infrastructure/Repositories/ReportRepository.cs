using AIEnterpriseCommandCenter.Application.DTOs.Asset;
using AIEnterpriseCommandCenter.Application.DTOs.Employee;
using AIEnterpriseCommandCenter.Application.DTOs.Project;
using AIEnterpriseCommandCenter.Application.DTOs.ServiceDesk;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================================================
        // Employee Report
        // ===================================================

        public async Task<List<EmployeeDto>> GetEmployeeReportAsync()
        {
            return await _context.Employees
         .OrderBy(x => x.FirstName)
         .Select(x => new EmployeeDto
         {
             Id = x.Id,
             EmployeeCode = x.EmployeeCode,
             FirstName = x.FirstName,
             LastName = x.LastName,
             Email = x.Email,
             PhoneNumber = x.PhoneNumber,
             Department = x.Department,
             Designation = x.Designation,
             JoiningDate = x.JoiningDate,
             Salary = x.Salary,
             IsActive = x.IsActive
         
          })
        .ToListAsync();
}

        // ===================================================
        // Asset Report
        // ===================================================

        public async Task<List<AssetDto>> GetAssetReportAsync()
        {
            return await _context.Assets
        .Include(x => x.Employee)
        .OrderBy(x => x.AssetName)
        .Select(x => new AssetDto
        {
            Id = x.Id,
            AssetCode = x.AssetCode,
            AssetName = x.AssetName,
            Category = x.Category,
            Brand = x.Brand,
            Model = x.Model,
            PurchaseDate = x.PurchaseDate,
            PurchasePrice = x.PurchasePrice,
            Status = x.Status,
            EmployeeId = x.EmployeeId,
            EmployeeName = x.Employee != null ? x.Employee.FullName : ""
        })
        .ToListAsync();
        }

        // ===================================================
        // Project Report
        // ===================================================

        public async Task<List<ProjectDto>> GetProjectReportAsync()
        {
            return await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees)
                .OrderBy(x => x.ProjectName)
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
                .ToListAsync();
        }

        // ===================================================
        // Ticket Report
        // ===================================================

        public async Task<List<TicketDto>> GetTicketReportAsync()
        {
            return await _context.ServiceTickets
        .Include(x => x.Employee)
        .OrderByDescending(x => x.CreatedOn)
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
        .ToListAsync();
        }

        public async Task<List<EmployeeDto>> ExportEmployeesAsync()
        {
            return await _context.Employees
                .OrderBy(x => x.FirstName)
                .Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Department = x.Department,
                    Designation = x.Designation,
                    PhoneNumber = x.PhoneNumber,
                    JoiningDate = x.JoiningDate,
                    Salary = x.Salary,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<List<AssetDto>> ExportAssetsAsync()
        {
            return await _context.Assets
                .Include(x => x.Employee)
                .OrderBy(x => x.AssetName)
                .Select(x => new AssetDto
                {
                    Id = x.Id,
                    AssetCode = x.AssetCode,
                    AssetName = x.AssetName,
                    Category = x.Category,
                    Brand = x.Brand,
                    Model = x.Model,
                    PurchaseDate = x.PurchaseDate,
                    PurchasePrice = x.PurchasePrice,
                    Status = x.Status,
                    EmployeeName = x.Employee != null
                        ? x.Employee.FullName
                        : "-"
                })
                .ToListAsync();
        }

        public async Task<List<ProjectDto>> ExportProjectsAsync()
        {
            return await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.ProjectEmployees)
                .OrderBy(x => x.ProjectName)
                .Select(x => new ProjectDto
                {
                    Id = x.Id,
                    ProjectCode = x.ProjectCode,
                    ProjectName = x.ProjectName,
                    ManagerName = x.Manager.FullName,
                    Priority = x.Priority,
                    Status = x.Status,
                    Progress = x.Progress,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TeamMembers = x.ProjectEmployees.Count()
                })
                .ToListAsync();
        }

        public async Task<List<TicketDto>> ExportTicketsAsync()
        {
            return await _context.ServiceTickets
                .Include(x => x.Employee)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new TicketDto
                {
                    Id = x.Id,
                    TicketNumber = x.TicketNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Category = x.Category,
                    Priority = x.Priority,
                    Status = x.Status,
                    EmployeeName = x.Employee.FullName,
                    AssignedTo = x.AssignedTo,
                    CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }
    }
}