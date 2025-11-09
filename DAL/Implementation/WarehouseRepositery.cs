using AppModels.Entities;
using DAL.Interface;

namespace DAL.Implementation
{
    public class WarehouseRepositery(FAContext context) : BaseRepo<Warehouse>(context), IWarehouseRepositery
    {
    }
}
