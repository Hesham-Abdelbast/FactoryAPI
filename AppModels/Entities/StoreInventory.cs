
using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    /// <summary>
    /// Represents the current inventory levels for each material type in the store
    /// </summary>
    public class StoreInventory : BaseEntity
    {

        /// <summary>
        /// Foreign key for the material type
        /// </summary>
        [Required]
        public Guid MaterialTypeId { get; set; }

        /// <summary>
        /// Navigation property for the material type
        /// </summary>
        public MaterialType MaterialType { get; set; }

        /// <summary>
        /// Current quantity of the material in stock
        /// </summary>
        public decimal CurrentQuantity { get; set; }
    }
}
