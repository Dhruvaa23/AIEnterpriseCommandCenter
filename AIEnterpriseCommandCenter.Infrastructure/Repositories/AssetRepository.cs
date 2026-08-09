using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnterpriseCommandCenter.Application.DTOs.Asset;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AIEnterpriseCommandCenter.Application.Common;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;

        public AssetRepository(
            ApplicationDbContext context,
            INotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<AssetDto>> GetAllAsync()
        {
            return await _context.Assets
                .Include(a => a.Employee)
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    Category = a.Category,
                    Brand = a.Brand,
                    Model = a.Model,
                    PurchaseDate = a.PurchaseDate,
                    PurchasePrice = a.PurchasePrice,
                    Status = a.Status,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null
                        ? a.Employee.FirstName + " " + a.Employee.LastName
                        : "-"
                })
                .ToListAsync();
        }

        public async Task<AssetDto?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.Employee)
                .Where(a => a.Id == id)
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    Category = a.Category,
                    Brand = a.Brand,
                    Model = a.Model,
                    PurchaseDate = a.PurchaseDate,
                    PurchasePrice = a.PurchasePrice,
                    Status = a.Status,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null
                        ? a.Employee.FirstName + " " + a.Employee.LastName
                        : "-"
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(CreateAssetDto dto)
        {
            var asset = new Asset
            {
                AssetCode = dto.AssetCode,
                AssetName = dto.AssetName,
                Category = dto.Category,
                Brand = dto.Brand,
                Model = dto.Model,
                PurchaseDate = dto.PurchaseDate,
                PurchasePrice = dto.PurchasePrice,
                Status = "Available"
            };

            _context.Assets.Add(asset);

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "New Asset Added",
                $"{asset.AssetName} ({asset.AssetCode}) added to inventory.",
                "Asset");
        }

        public async Task UpdateAsync(UpdateAssetDto dto)
        {
            var asset = await _context.Assets.FindAsync(dto.Id);

            if (asset == null)
                return;

            asset.AssetCode = dto.AssetCode;
            asset.AssetName = dto.AssetName;
            asset.Category = dto.Category;
            asset.Brand = dto.Brand;
            asset.Model = dto.Model;
            asset.PurchaseDate = dto.PurchaseDate;
            asset.PurchasePrice = dto.PurchasePrice;
            asset.Status = dto.Status;
            asset.EmployeeId = dto.EmployeeId;

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
    "Asset Updated",
    $"{asset.AssetName} information was updated.",
    "Asset");
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);

            if (asset == null)
                return;

            var assetName = asset.AssetName;

            _context.Assets.Remove(asset);

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Asset Deleted",
                $"{assetName} has been removed from inventory.",
                "Asset");
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Assets.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AssetDto>> SearchAsync(string searchTerm)
        {
            return await _context.Assets
                .Include(a => a.Employee)
                .Where(a =>
                    a.AssetCode.Contains(searchTerm) ||
                    a.AssetName.Contains(searchTerm) ||
                    a.Category.Contains(searchTerm))
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    Category = a.Category,
                    Brand = a.Brand,
                    Model = a.Model,
                    PurchaseDate = a.PurchaseDate,
                    PurchasePrice = a.PurchasePrice,
                    Status = a.Status,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null
                        ? a.Employee.FirstName + " " + a.Employee.LastName
                        : "-"
                })
                .ToListAsync();
        }

        public async Task AssignAssetAsync(int assetId, int employeeId)
        {
            var asset = await _context.Assets
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == assetId);

            if (asset == null)
                return;

            asset.EmployeeId = employeeId;
            asset.Status = "Assigned";

            await _context.SaveChangesAsync();

            var employee = await _context.Employees.FindAsync(employeeId);

            await _notificationRepository.CreateAsync(
                "Asset Assigned",
                $"{asset.AssetName} assigned to {employee?.FullName}.",
                "Asset");
        }

        public async Task ReturnAssetAsync(int assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);

            if (asset == null)
                return;

            var assetName = asset.AssetName;

            asset.EmployeeId = null;
            asset.Status = "Available";

            await _context.SaveChangesAsync();

            await _notificationRepository.CreateAsync(
                "Asset Returned",
                $"{assetName} has been returned and is now available.",
                "Asset");
        }

        public async Task<AssetDashboardDto> GetDashboardAsync()
        {
            return new AssetDashboardDto
            {
                TotalAssets = await _context.Assets.CountAsync(),

                AvailableAssets = await _context.Assets
                    .CountAsync(x => x.Status == "Available"),

                AssignedAssets = await _context.Assets
                    .CountAsync(x => x.Status == "Assigned"),

                TotalAssetValue = await _context.Assets
                    .SumAsync(x => x.PurchasePrice)
            };
        }

        public async Task<IEnumerable<AssetDto>> FilterAsync(AssetFilterDto filter)
        {
            var query = _context.Assets
                .Include(x => x.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.AssetName.Contains(filter.Search) ||
                    x.AssetCode.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                query = query.Where(x => x.Category == filter.Category);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            if (!string.IsNullOrWhiteSpace(filter.Brand))
            {
                query = query.Where(x => x.Brand == filter.Brand);
            }

            return await query.Select(x => new AssetDto
            {
                Id = x.Id,
                AssetCode = x.AssetCode,
                AssetName = x.AssetName,
                Category = x.Category,
                Brand = x.Brand,
                Status = x.Status,
                EmployeeName = x.Employee != null
                    ? x.Employee.FullName
                    : "-"
            }).ToListAsync();
        }

        public async Task<PagedResult<AssetDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Assets
                .Include(x => x.Employee);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.AssetCode)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                
                .Select(x => new AssetDto
                {
                    Id = x.Id,
                    AssetCode = x.AssetCode,
                    AssetName = x.AssetName,
                    Category = x.Category,
                    Brand = x.Brand,
                    Status = x.Status,
                    EmployeeName = x.Employee != null
                        ? x.Employee.FullName
                        : "-"
                })
                .ToListAsync();

            return new PagedResult<AssetDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<List<AssetDto>> ExportAsync()
        {
            return await _context.Assets
                .Include(x => x.Employee)
                .Select(x => new AssetDto
                {
                    Id = x.Id,
                    AssetCode = x.AssetCode,
                    AssetName = x.AssetName,
                    Category = x.Category,
                    Brand = x.Brand,
                    Status = x.Status,
                    EmployeeName = x.Employee != null
                        ? x.Employee.FullName
                        : "-"
                })
                .ToListAsync();
        }

        public async Task<List<AssetDto>> GetAllAssetsAsync()
        {
            return await _context.Assets
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    Category = a.Category,
                    Brand = a.Brand,
                    Model = a.Model,
                    Status = a.Status,
                    PurchaseDate = a.PurchaseDate,
                    PurchasePrice = a.PurchasePrice,
                    EmployeeId = a.EmployeeId
                })
                .ToListAsync();
        }

        public async Task<AssetDto?> GetByAssetCodeAsync(string assetCode)
        {
            assetCode = assetCode.Trim().ToUpper();

            return await _context.Assets
                .Where(a => a.AssetCode.ToUpper() == assetCode)
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetCode = a.AssetCode,
                    AssetName = a.AssetName,
                    Category = a.Category,
                    Brand = a.Brand,
                    Model = a.Model,
                    Status = a.Status,
                    PurchaseDate = a.PurchaseDate,
                    PurchasePrice = a.PurchasePrice,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null
                ? a.Employee.FirstName + " " + a.Employee.LastName
                : null
                })
                .FirstOrDefaultAsync();
        }
        public async Task<int> GetAssetCountAsync()
        {
            return await _context.Assets.CountAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Assets.CountAsync();
        }

        public async Task<int> GetAvailableCountAsync()
        {
            return await _context.Assets
                .CountAsync(x => x.Status == "Available");
        }

        public async Task<int> GetAssignedCountAsync()
        {
            return await _context.Assets
                .CountAsync(x => x.EmployeeId != null);
        }

        public async Task<int> GetAvailableAssetCountAsync()
        {
            return await _context.Assets
                .CountAsync(x => x.Status == "Available");
        }
    }
}
