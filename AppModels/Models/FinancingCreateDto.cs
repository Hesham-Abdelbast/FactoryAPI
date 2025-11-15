namespace AppModels.Models
{
    public sealed class FinancingCreateDto
    {
        public decimal Amount { get; set; }
        public string ProviderName { get; set; } = default!;
        public string? Notes { get; set; }
    }
}
