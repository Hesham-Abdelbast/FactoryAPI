namespace AppModels.Models.MerchantMangement
{
    public class MerchantExpenseDto
    {
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
