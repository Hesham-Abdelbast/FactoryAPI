using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.MerchantMangement
{
    /// <summary>
    /// يمثل مصاريف التاجر
    /// </summary>
    public class MerchantExpense : BaseEntity
    {
        /// <summary>
        /// معرف التاجر
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }

        /// <summary>
        /// المبلغ المصروف
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// سبب المصاريف
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// تاريخ المصاريف
        /// </summary>
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// حالة الحذف الناعم
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// التاجر المرتبط بالمصاريف
        /// </summary>
        public Merchant Merchant { get; set; }
    }
}
