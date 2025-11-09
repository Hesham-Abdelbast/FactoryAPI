using AppModels.Entities;
using DAL.Interface;

namespace DAL.Implementation
{
    public class WarehouseInventoryRepo(FAContext context) : BaseRepo<WarehouseInventory>(context), IWarehouseInventoryRepo
    {

    }
}
