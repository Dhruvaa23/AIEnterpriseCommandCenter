

using AIEnterpriseCommandCenter.Application.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace AIEnterpriseCommandCenter.Application.Services;

public class AssetAITool : IAssetAITool
{
    private readonly IAssetRepository _assetRepository;

    public AssetAITool(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<string?> ExecuteAsync(string prompt)
    {
        prompt = prompt.ToLower().Trim();

        var assets = await _assetRepository.GetAllAssetsAsync();

        //------------------------------------
        // Asset Count
        //------------------------------------

        if (prompt.Contains("how many assets") ||
            prompt.Contains("asset count") ||
            prompt.Contains("total assets"))
        {
            return $"There are currently {assets.Count} assets in the company.";
        }

        //------------------------------------
        // List Assets
        //------------------------------------

        if (prompt.Contains("list assets") ||
            prompt.Contains("show assets") ||
            prompt.Contains("all assets"))
        {
            if (!assets.Any())
                return "No assets found.";

            StringBuilder sb = new();

            sb.AppendLine($"Assets ({assets.Count})");
            sb.AppendLine("--------------------------------");

            foreach (var asset in assets)
            {
                sb.AppendLine($"{asset.AssetCode} | {asset.AssetName} | {asset.Status}");
            }

            return sb.ToString();
        }

        //------------------------------------
        // Available Assets Count
        //------------------------------------

        if (prompt.Contains("available asset count"))
        {
            var available = assets.Count(x =>
                x.Status.Equals("Available", StringComparison.OrdinalIgnoreCase));

            return $"There are currently {available} available assets.";
        }

        //------------------------------------
        // Available Assets List
        //------------------------------------

        if (prompt.Contains("available assets") ||
            prompt.Contains("available asset"))
        {
            var available = assets
                .Where(x => x.Status.Equals("Available", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!available.Any())
                return "No available assets found.";

            StringBuilder sb = new();

            sb.AppendLine($"Available Assets ({available.Count})");
            sb.AppendLine("--------------------------------");

            foreach (var asset in available)
            {
                sb.AppendLine($"{asset.AssetCode} - {asset.AssetName}");
            }

            return sb.ToString();
        }

        //------------------------------------
        // Assigned Assets
        //------------------------------------

        if (prompt.Contains("assigned assets"))
        {
            var assigned = assets
                .Where(x => x.Status.Equals("Assigned", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!assigned.Any())
                return "No assigned assets found.";

            StringBuilder sb = new();

            sb.AppendLine($"Assigned Assets ({assigned.Count})");
            sb.AppendLine("--------------------------------");

            foreach (var asset in assigned)
            {
                sb.AppendLine($"{asset.AssetCode} - {asset.AssetName} ({asset.EmployeeName})");
            }

            return sb.ToString();
        }

        //------------------------------------
        // Category Search
        //------------------------------------

        string[] categories =
        {
            "laptop",
            "desktop",
            "monitor",
            "printer",
            "mobile"
        };

        foreach (var category in categories)
        {
            if (prompt.Contains(category))
            {
                var result = assets
                    .Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!result.Any())
                    return $"No {category} assets found.";

                StringBuilder sb = new();

                sb.AppendLine($"{category.ToUpper()} Assets");
                sb.AppendLine("--------------------------------");

                foreach (var asset in result)
                {
                    sb.AppendLine($"{asset.AssetCode} - {asset.AssetName} ({asset.Status})");
                }

                return sb.ToString();
            }
        }

        //------------------------------------
        // Asset Details
        //------------------------------------

        var match = Regex.Match(prompt, @"ast\d+", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            var code = match.Value.ToUpper();

            var asset = await _assetRepository.GetByAssetCodeAsync(code);

            if (asset == null)
                return $"Asset {code} not found.";

            return
$"""
Asset Information
────────────────────────

Asset Code    : {asset.AssetCode}

Asset Name    : {asset.AssetName}

Category      : {asset.Category}

Brand         : {asset.Brand}

Model         : {asset.Model}

Status        : {asset.Status}

Assigned To   : {asset.EmployeeName ?? "Not Assigned"}

Purchase Date : {asset.PurchaseDate:dd-MMM-yyyy}

Price         : ₹{asset.PurchasePrice:N2}
""";
        }

        //------------------------------------
        // Unknown Query
        //------------------------------------

        return null;
    }
}