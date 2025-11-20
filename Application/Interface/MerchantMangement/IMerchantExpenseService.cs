using AppModels.Common;
using AppModels.Models.MerchantMangement;
using Ejd.GRC.AppModels.Common;
namespace Application.Interface.MerchantMangement
{
    public interface IMerchantExpenseService
    {
        Task<IEnumerable<MerchantExpenseDto>> GetAllAsync(PaginationEntity param);
        Task<PagedResult<MerchantExpenseDto>> GetAllByMerchantIdAsync(Guid merchantId, PaginationEntity param);
        Task<IEnumerable<MerchantExpenseDto>> GetAllAsync();
        Task<MerchantExpenseDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(MerchantExpenseCreateDto dto);
        Task<bool> UpdateAsync(MerchantExpenseDto dto);
        Task<bool> DeleteAsync(Guid id);

        Task<decimal> GetMerchantExpenseSummaryAsync(Guid merchantId, ExpenseSummaryRequest request);
    }
}
