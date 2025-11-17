using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Employees
{
    /// <summary>
    /// DTO للسُلف المالية التي يستلمها الموظف
    /// </summary>
    public class EmployeeCashAdvanceDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// رقم معرف الموظف
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// قيمة السُلفة المالية
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// ملاحظة اختيارية
        /// </summary>
        public string? Note { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
