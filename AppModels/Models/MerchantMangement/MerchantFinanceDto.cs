using AppModels.Entities.MerchantMangement;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.MerchantMangement
{
    public sealed class MerchantFinanceDto
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public DateTime OperationDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}
