
using AppModels.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation
{
    public class MerchantRepository(FAContext context) : BaseRepo<Merchant>(context), IMerchantRepository
    {
      
    }
}
