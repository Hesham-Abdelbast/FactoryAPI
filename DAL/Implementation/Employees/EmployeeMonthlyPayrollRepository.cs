using AppModels.Entities.Employees;
using DAL.Interface.Employees;

namespace DAL.Implementation.Employees
{
    public class EmployeeMonthlyPayrollRepository(FAContext context)
        : BaseRepo<EmployeeMonthlyPayroll>(context), IEmployeeMonthlyPayrollRepository
    {
    }
}
