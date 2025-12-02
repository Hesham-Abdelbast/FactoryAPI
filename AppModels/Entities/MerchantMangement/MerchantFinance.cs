using AppModels.Common;
using AppModels.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.MerchantMangement
{
    public class MerchantFinance : BaseEntity
    {
        [Required]
        public Guid MerchantId { get; set; }
        public Merchant Merchant { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime OperationDate { get; set; } = DateTime.UtcNow;

        [StringLength(300)]
        public string? Notes { get; set; }
    }
}