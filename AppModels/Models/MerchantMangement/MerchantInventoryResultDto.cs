using AppModels.Models.Transaction;

namespace AppModels.Models.MerchantMangement
{
    public sealed class MerchantInventoryResultDto
    {
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetBalance => TotalSales - TotalExpenses;
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
        public List<MerchantExpenseDto> MerchantExpenses { get; set; } = new List<MerchantExpenseDto>();

    }
}
