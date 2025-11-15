using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Employees
{
    public class EmployeeMonthlyPayroll : BaseEntity
    {

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        public int Year { get; set; }
        public int Month { get; set; } // 1..12

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BaseSalary { get; set; } // المرتب لهذا الشهر (يمكن نسخه من Employee.BaseSalary)

        public decimal PersonalExpensesTotal { get; set; } // مجموع المصاريف الشخصية للشهر


        public decimal CashAdvancesTotal { get; set; } // مجموع السُلف/الرصيد المستلم للشهر

        // الباقي من المرتب = المرتب - السُلف - المصاريف
        public decimal Remaining => BaseSalary - CashAdvancesTotal - PersonalExpensesTotal;
    }
}
