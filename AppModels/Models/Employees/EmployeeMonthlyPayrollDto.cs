namespace AppModels.Models.Employees
{
    /// <summary>
    /// DTO لنتائج الكشف المالي الشهري للموظف
    /// </summary>
    public class EmployeeMonthlyPayrollDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// رقم معرف الموظف
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// السنة
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// الشهر
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// المرتب قبل الخصومات أو الإضافات
        /// </summary>
        public decimal BaseSalary { get; set; }

        /// <summary>
        /// إجمالي المصاريف الشخصية خلال الشهر
        /// </summary>
        public decimal PersonalExpensesTotal { get; set; }

        /// <summary>
        /// إجمالي السُلف خلال الشهر
        /// </summary>
        public decimal CashAdvancesTotal { get; set; }

        /// <summary>
        /// المتبقي من الراتب بعد الخصومات
        /// </summary>
        public decimal Remaining => BaseSalary - PersonalExpensesTotal - CashAdvancesTotal;
    }
}
