using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Transaction
{
    /// <summary>
    /// Data Transfer Object representing a material transaction between the factory and a merchant.
    /// </summary>
    public sealed class TransactionDto
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// External system reference or custom code for the transaction.
        /// </summary>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        /// Type of transaction (Income / Outcome).
        /// </summary>
        public TransactionType Type { get; set; }
        public string TypeNameAr { get; set; }

        /// <summary>
        /// ID of the material type used in the transaction.
        /// </summary>
        public Guid MaterialTypeId { get; set; }

        /// <summary>
        /// Name of the material type (from navigation).
        /// </summary>
        public string? MaterialTypeName { get; set; }

        #region truck
        /// <summary>
        /// Name of the car driver delivering the material.
        /// </summary>
        public string? CarDriverName { get; set; }
        /// <summary>
        /// Total weight of the truck and material.
        /// </summary>
        public decimal CarAndMatrerialWeight { get; set; }

        /// <summary>
        /// Weight of the empty truck.
        /// </summary>
        public decimal CarWeight { get; set; }

        /// <summary>
        /// Actual material quantity being received or sold.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Percentage of impurities in the material.
        /// </summary>
        public decimal PercentageOfImpurities { get; set; }

        /// <summary>
        /// Total weight of impurities removed from the material.
        /// </summary>
        public decimal WeightOfImpurities { get; set; }

        /// <summary>
        /// Price per unit of material.
        /// </summary>
        #endregion

        public decimal PricePerUnit { get; set; }

        /// <summary>
        /// Total monetary value of the transaction.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// ID of the merchant associated with the transaction.
        /// </summary>
        public Guid MerchantId { get; set; }
        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }

        /// <summary>
        /// Name of the merchant (from navigation).
        /// </summary>
        public string? MerchantName { get; set; }

        /// <summary>
        /// Additional notes or remarks about the transaction.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Amount already paid for this transaction.
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Remaining amount still unpaid.
        /// </summary>
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// Indicates whether the transaction is fully paid.
        /// </summary>
        public bool IsFullyPaid { get; set; }

        /// <summary>
        /// Timestamp for when the transaction was created.
        /// </summary>
        public DateTime CreateDate { get; set; }
        public bool ShowPhoneNumber { get; set; }
    }

}
