namespace AppModels.Models.Warehouse
{
    public sealed class WarehouseInventoryDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        public Guid MaterialTypeId { get; set; }
        public string? MaterialTypeName {  get; set; }

        public decimal CurrentQuantity { get; set; }
    }
}
