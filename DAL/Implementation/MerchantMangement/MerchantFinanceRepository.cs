using AppModels.Entities.MerchantMangement;
using DAL.Interface.MerchantMangement;

namespace DAL.Implementation.MerchantMangement
{
    public class MerchantFinanceRepository(FAContext context) : BaseRepo<MerchantFinance>(context), IMerchantFinanceRepository
    {
    }
}
