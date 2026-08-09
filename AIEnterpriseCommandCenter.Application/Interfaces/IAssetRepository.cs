using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIEnterpriseCommandCenter.Application.DTOs.Asset;
using AIEnterpriseCommandCenter.Application.Common;


namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IAssetRepository
    {
        Task<IEnumerable<AssetDto>> GetAllAsync();

        Task<AssetDto?> GetByIdAsync(int id);

        Task AddAsync(CreateAssetDto dto);

        Task UpdateAsync(UpdateAssetDto dto);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<AssetDto>> SearchAsync(string searchTerm);

        Task AssignAssetAsync(int assetId, int employeeId);

        Task ReturnAssetAsync(int assetId);

        Task<AssetDashboardDto> GetDashboardAsync();

        Task<IEnumerable<AssetDto>> FilterAsync(AssetFilterDto filter);

        Task<PagedResult<AssetDto>> GetPagedAsync(int pageNumber, int pageSize);

        Task<List<AssetDto>> ExportAsync();

        Task<List<AssetDto>> GetAllAssetsAsync();

        Task<AssetDto?> GetByAssetCodeAsync(string assetCode);

        Task<int> GetAssetCountAsync();

        Task<int> GetCountAsync();

        Task<int> GetAvailableCountAsync();

        Task<int> GetAssignedCountAsync();

        Task<int> GetAvailableAssetCountAsync();

    }
}
