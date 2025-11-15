using AppModels.Entities.Store;
using DAL.Interface;

namespace DAL.Implementation.Store
{
    public class WarehouseExpenseRepository(FAContext context)
        : BaseRepo<WarehouseExpense>(context), IWarehouseExpenseRepository
    {
    }
}
