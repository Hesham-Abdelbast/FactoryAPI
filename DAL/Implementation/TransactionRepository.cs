using AppModels.Entities;
using DAL.Interface;

namespace DAL.Implementation
{
    public class TransactionRepository(FAContext context) : BaseRepo<Transaction>(context), ITransactionRepository
    {
        
    }
}
