using AppModels.Common;

namespace AppModels.Entities
{
    public class WarehouseInventory : BaseEntity
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public Guid MaterialTypeId { get; set; }
        public MaterialType MaterialType { get; set; }
        public decimal CurrentQuantity { get; set; }

        /// <summary>
        /// Optional notes or description.
        /// </summary>
        public string Notes { get; set; }
    }
}
