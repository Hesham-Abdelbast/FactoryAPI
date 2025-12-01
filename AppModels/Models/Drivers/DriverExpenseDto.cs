namespace AppModels.Models.Drivers
{
    public sealed class DriverExpenseDto
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
