

using AppModels.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation
{
    public class StoreInventoryRepository(FAContext context) : BaseRepo<StoreInventory>(context), IStoreInventoryRepository
    {
       
    }
}
