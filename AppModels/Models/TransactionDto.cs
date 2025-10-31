using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models
{
    public sealed class TransactionDto
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// Type of transaction (Income/Outcome)
        /// </summary>
        [Required]
        public int Type { get; set; }

        public string TransactionIdentifier { get; set; }

        /// <summary>
        /// Foreign key for the material type involved in the transaction
        /// </summary>
        [Required]
        public Guid MaterialTypeId { get; set; }

        /// <summary>
        /// Quantity of material in the transaction
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Price per unit of the material
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }

        /// <summary>
        /// Total amount of the transaction (calculated property)
        /// </summary>
        public decimal TotalAmount => Quantity * PricePerUnit;

        /// <summary>
        /// Foreign key for the merchant involved in the transaction
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Additional description or notes about the transaction
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Amount already paid to or by the merchant
        /// </summary>
        public decimal AmountPaid { get; set; } = 0;

    }
}
