namespace AppModels.Models.Employees
{
    public sealed class EmployeeFullFinancialReportDto
    {
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal BaseSalary { get; set; }

        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }

        public decimal TotalCashAdvances { get; set; }
        public decimal TotalPersonalExpenses { get; set; }

        public IEnumerable<EmployeeMonthlyPayrollDto>? PayrollHistory { get; set; }
    }
}
