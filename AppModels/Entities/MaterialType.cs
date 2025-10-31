using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    /// <summary>
    /// Represents the type of material in the factory management system
    /// </summary>
    public class MaterialType : BaseEntity
    {
        /// <summary>
        /// Name of the material type (Iron, Aluminum, Copper)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the material type
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property for transactions involving this material type
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <summary>
        /// Navigation property for store inventory of this material type
        /// </summary>
        public StoreInventory StoreInventory { get; set; }
    }
}
