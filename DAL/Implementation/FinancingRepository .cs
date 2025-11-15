using AppModels.Entities;
using DAL.Interface;

namespace DAL.Implementation
{
    public class FinancingRepository(FAContext context) : BaseRepo<Financing>(context), IFinancingRepository
    {
    }
}
