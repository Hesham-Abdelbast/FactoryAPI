using AppModels.Models;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface
{
    public interface ITransactionServices
    {
        // Create
        Task<Guid> AddAsync(TransactionDto entity);

        Task<IEnumerable<TransactionDto>> GetAllByMerchantIdAsync(Guid merchantId);

        // Read (Get by Id)
        Task<TransactionDto?> GetByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<TransactionDto>> GetAllAsync(PaginationEntity param);

        // Update
        Task<bool> UpdateAsync(TransactionDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);
    }
}
