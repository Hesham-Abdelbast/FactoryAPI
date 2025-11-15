using AppModels.Entities.Store;
using DAL.Interface;

namespace DAL.Implementation.Store
{
    public class WarehouseInventoryRepo(FAContext context) : BaseRepo<WarehouseInventory>(context), IWarehouseInventoryRepo
    {

    }
}
