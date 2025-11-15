using AppModels.Entities.Employees;
using DAL.Interface.Employees;

namespace DAL.Implementation.Employees
{
    public class EmployeePersonalExpenseRepository(FAContext context)
        : BaseRepo<EmployeePersonalExpense>(context), IEmployeePersonalExpenseRepository
    {
    }
}
