using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Employees
{
    /// <summary>
    /// DTO للمصاريف الشخصية التي يصرفها الموظف أثناء العمل
    /// </summary>
    public class EmployeePersonalExpenseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// رقم معرف الموظف
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// قيمة المصروف
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
