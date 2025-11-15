using AppModels.Entities.MerchantMangement;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.MerchantMangement
{
    public class MerchantRepository(FAContext context) : BaseRepo<Merchant>(context), IMerchantRepository
    {
      
    }
}
