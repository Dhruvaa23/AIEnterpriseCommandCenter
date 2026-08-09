using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Asset
{
    public class AssetDashboardDto
    {
        public int TotalAssets { get; set; }

        public int AvailableAssets { get; set; }

        public int AssignedAssets { get; set; }

        public decimal TotalAssetValue { get; set; }
    }
}
