using AppModels.Common;
using AppModels.Models.MerchantMangement;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface.MerchantMangement
{
    public interface IMerchantServices
    {
        // Create
        Task<Guid> AddAsync(MerchantDto entity);

        // Read (Get by Id)
        Task<MerchantDto?> GetByIdAsync(Guid id);

        // Read (Get all) بدون Pagination
        Task<IEnumerable<MerchantDto>> GetAllAsync();

        // Read (Get all) مع Pagination
        Task<PagedResult<IEnumerable<MerchantDto>>> GetAllAsync(PaginationEntity param);

        // Update
        Task<bool> UpdateAsync(MerchantDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);
    }
}
