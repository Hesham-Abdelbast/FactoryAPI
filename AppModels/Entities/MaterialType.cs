using AppModels.Common;
using AppModels.Common.Enums;
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

        public MaterialCategory Type { get; set; }

        /// <summary>
        /// Navigation property for transactions involving this material type
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
