namespace AppModels.Models.Employees
{
    /// <summary>
    /// تقرير مالي مجمع للموظف خلال فترة زمنية
    /// </summary>
    public class EmployeeFinancialReportDto
    {
        /// <summary>
        /// رقم معرف الموظف
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// إجمالي السُلف المالية خلال الفترة
        /// </summary>
        public decimal TotalCashAdvances { get; set; }

        /// <summary>
        /// إجمالي المصاريف الشخصية خلال الفترة
        /// </summary>
        public decimal TotalPersonalExpenses { get; set; }

        /// <summary>
        /// إجمالي الخصومات
        /// </summary>
        public decimal TotalSpent => TotalCashAdvances + TotalPersonalExpenses;
    }
}
