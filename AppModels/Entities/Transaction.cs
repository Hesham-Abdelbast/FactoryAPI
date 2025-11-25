using AppModels.Common;
using AppModels.Entities.MerchantMangement;
using AppModels.Entities.Store;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    /// <summary>
    /// Represents a transaction of materials between the factory and a merchant
    /// </summary>
    public class Transaction : BaseEntity
    {
        /// <summary>
        /// external system code for the transaction
        /// </summary>
        public string TransactionIdentifier { get; set; } = string.Empty;
        /// <summary>
        /// Type of transaction (Income/Outcome)
        /// </summary>
        [Required]
        public TransactionType Type { get; set; }
        /// <summary>
        /// Only Input when the transaction is Outcome
        /// </summary>
        public decimal? PricePerSelling { get; set; }

        /// <summary>
        /// Foreign key for the material type involved in the transaction
        /// </summary>
        [Required]
        public Guid MaterialTypeId { get; set; }

        /// <summary>
        /// Navigation property for the material type
        /// </summary>
        public MaterialType MaterialType { get; set; }

        /// <summary>
        /// Foreign key for the Warehouse involved in the transaction
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Navigation property for the Warehouse
        /// </summary>
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// Name of the car driver delivering the material.
        /// </summary>
        [StringLength(200)]
        public string? CarDriverName { get; set; }
        /// <summary>
        /// weight of Car And Matreria in the transaction
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal CarAndMatrerialWeight { get; set; }

        /// <summary>
        /// weight of Car in the transaction
        /// </summary>
        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal CarWeight { get; set; }

        /// <summary>
        /// Quantity of material in the transaction
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PercentageOfImpurities { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal WeightOfImpurities { get; set; }
        /// <summary>
        /// Price per unit of the material
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerUnit { get; set; }

        /// <summary>
        /// Total amount of the transaction (calculated property)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Foreign key for the merchant involved in the transaction
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Navigation property for the merchant
        /// </summary>
        public Merchant Merchant { get; set; }

        /// <summary>
        /// Additional description or notes about the transaction
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Amount already paid to or by the merchant
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Remaining amount to be paid (calculated property)
        /// </summary>
        public decimal RemainingAmount => TotalAmount - AmountPaid;

        /// <summary>
        /// Indicates whether the transaction is fully paid
        /// </summary>
        public bool IsFullyPaid => RemainingAmount <= 0;

        public bool ShowPhoneNumber { get; set; } = false;
    }
}
