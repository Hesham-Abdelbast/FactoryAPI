using AppModels.Common;
using AppModels.Models.MerchantMangement;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface.MerchantMangement
{
    public interface IMerchantFinanceServices
    {
        Task<PagedResult<IEnumerable<MerchantFinanceDto>>> GetAllAsync(PaginationEntity param);
        Task<IEnumerable<MerchantFinanceDto>> GetAllAsync();
        Task<PagedResult<IEnumerable<MerchantFinanceDto>>> GetAllByMerchantIdAsync(Guid merchantId, PaginationEntity param);
        Task<MerchantFinanceDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(MerchantFinanceDto dto);
        Task<bool> UpdateAsync(MerchantFinanceDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
