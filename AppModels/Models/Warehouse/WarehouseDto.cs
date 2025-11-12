namespace AppModels.Models.Warehouse
{
    public sealed class WarehouseDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Location { get; set; }

        // ✅ General Info
        public string? ManagerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
