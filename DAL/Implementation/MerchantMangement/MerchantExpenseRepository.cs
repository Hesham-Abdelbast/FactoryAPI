using AppModels.Entities.MerchantMangement;
using DAL.Interface.MerchantMangement;

namespace DAL.Implementation.MerchantMangement
{
    public class MerchantExpenseRepository(FAContext context) : BaseRepo<MerchantExpense>(context),IMerchantExpenseRepository
    {
    }
}
