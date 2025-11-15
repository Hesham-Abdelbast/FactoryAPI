namespace AppModels.Models
{
    public sealed class FinancingDto
    {
        public Guid? Id { get; set; }
        public decimal Amount { get; set; }
        public string ProviderName { get; set; } = default!;
        public string? Notes { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
