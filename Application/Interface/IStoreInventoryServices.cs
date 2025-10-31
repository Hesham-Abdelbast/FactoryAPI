using AppModels.Models;

namespace Application.Interface
{
    public interface IStoreInventoryServices
    {
        // Create
        Task<Guid> AddAsync(StoreSummaryDto entity);

        // Read (Get by Id)
        Task<StoreSummaryDto?> GetByMaterialTypeIdAsync(Guid materialTypeId);

        // Read (Get all)
        Task<IEnumerable<StoreSummaryDto>> GetAllAsync();

        // Update
        Task<bool> UpdateAsync(StoreSummaryDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);

       
    }
}
