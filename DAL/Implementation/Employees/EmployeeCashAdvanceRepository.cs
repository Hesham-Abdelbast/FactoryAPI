using AppModels.Entities.Employees;
using DAL.Interface.Employees;

namespace DAL.Implementation.Employees
{
    public class EmployeeCashAdvanceRepository(FAContext context)
        : BaseRepo<EmployeeCashAdvance>(context), IEmployeeCashAdvanceRepository
    {
    }
}
