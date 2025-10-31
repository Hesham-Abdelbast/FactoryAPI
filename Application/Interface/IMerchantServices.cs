using AppModels.Models;

namespace Application.Interface
{
    public interface IMerchantServices
    {
        // Create
        Task<Guid> AddAsync(MerchantDto entity);

        // Read (Get by Id)
        Task<MerchantDto?> GetByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<MerchantDto>> GetAllAsync();

        // Update
        Task<bool> UpdateAsync(MerchantDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);
    }
}
