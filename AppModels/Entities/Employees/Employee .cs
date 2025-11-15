using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Employees
{
    public class Employee : BaseEntity
    {

        [Required, MaxLength(150)]
        public string Name { get; set; } = default!;

        public DateOnly StartDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BaseSalary { get; set; } // المرتب

        public string? Notes { get; set; }

        // تنقلات
        public ICollection<EmployeePersonalExpense> PersonalExpenses { get; set; } = new List<EmployeePersonalExpense>();
        public ICollection<EmployeeCashAdvance> CashAdvances { get; set; } = new List<EmployeeCashAdvance>();
        public ICollection<EmployeeMonthlyPayroll> MonthlyPayrolls { get; set; } = new List<EmployeeMonthlyPayroll>();
    }
}
