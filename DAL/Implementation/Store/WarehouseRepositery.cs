using AppModels.Entities.Store;
using DAL.Interface;

namespace DAL.Implementation.Store
{
    public class WarehouseRepositery(FAContext context) : BaseRepo<Warehouse>(context), IWarehouseRepositery
    {
    }
}
