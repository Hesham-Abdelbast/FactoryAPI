using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Employees
{
    /// <summary>
    /// DTO لبيانات الموظف الأساسية
    /// </summary>
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// اسم الموظف
        /// </summary>
        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// تاريخ بداية العمل
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// المرتب الأساسي للموظف
        /// </summary>
        public decimal BaseSalary { get; set; }

        public string? Notes { get; set; }
    }
}
