using AppModels.Entities.Employees;
using DAL.Interface.Employees;
namespace DAL.Implementation.Employees
{
    public class EmployeeRepository(FAContext context)
        : BaseRepo<Employee>(context), IEmployeeRepository
    {
    }
}
