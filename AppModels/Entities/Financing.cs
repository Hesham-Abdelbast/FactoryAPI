using AppModels.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppModels.Entities
{
    /// <summary>
    /// يمثل دفعة تمويل ( تمويل) يقدمها ممول للمصنع لاستخدامه في المصروفات/التشغيل.
    /// </summary>
    public class Financing : BaseEntity 
    {
        /// <summary>
        /// مبلغ التمويل الكلي (المعطى من الممول).
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// اسم الممول (الشخص أو الكيان) الذي أعطى التمويل.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProviderName { get; set; } = default!;
      
        /// <summary>
        /// ملاحظات عامة.
        /// </summary>
        [MaxLength(2000)]
        public string? Notes { get; set; }
    }
}
