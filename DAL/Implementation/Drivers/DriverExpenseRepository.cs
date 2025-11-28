using AppModels.Entities.Drivers;
using DAL.Interface.Drivers;

namespace DAL.Implementation.Drivers
{
    public class DriverExpenseRepository(FAContext context)
        : BaseRepo<DriverExpense>(context), IDriverExpenseRepository
    {
    }
}
