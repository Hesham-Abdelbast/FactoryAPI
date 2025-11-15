namespace AppModels.Models.Store
{
    public sealed class WarehouseExpenseDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? Notes { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
