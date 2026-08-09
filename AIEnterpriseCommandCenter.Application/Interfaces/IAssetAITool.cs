using AIEnterpriseCommandCenter.Application.DTOs.Asset;
namespace AIEnterpriseCommandCenter.Application.Interfaces;

public interface IAssetAITool
{
    Task<string?> ExecuteAsync(string prompt);

}
