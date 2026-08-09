using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace AIEnterpriseCommandCenter.Application.DTOs.Asset
{
    public class CreateAssetDto
    {
        [Required]
        [MaxLength(30)]
        public string AssetCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AssetName { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }
    }
}
