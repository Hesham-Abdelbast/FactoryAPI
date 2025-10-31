using AppModels.Entities;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models
{
    /// <summary>
    /// Data transfer object for store summary information
    /// </summary>
    public class StoreSummaryDto
    {
        /// <summary>
        /// Foreign key for the material type
        /// </summary>
        [Required]
        public Guid MaterialTypeId { get; set; }

        public string MaterialTypeName { get; set; }

        /// <summary>
        /// Current quantity of the material in stock
        /// </summary>
        public decimal CurrentQuantity { get; set; }
    }
}
