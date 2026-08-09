using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Asset
{
    public class AssetFilterDto
    {
        public string? Search { get; set; }

        public string? Category { get; set; }

        public string? Status { get; set; }

        public string? Brand { get; set; }
    }
}
