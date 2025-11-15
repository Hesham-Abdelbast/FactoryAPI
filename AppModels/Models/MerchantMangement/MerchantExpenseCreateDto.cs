namespace AppModels.Models.MerchantMangement
{
    public sealed class MerchantExpenseCreateDto
    {
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime? ExpenseDate { get; set; }
    }
}
