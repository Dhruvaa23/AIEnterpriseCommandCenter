using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Asset
{
    public class AssetDto
    {
        public int Id { get; set; }

        public string AssetCode { get; set; } = string.Empty;

        public string AssetName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }
    }
}
