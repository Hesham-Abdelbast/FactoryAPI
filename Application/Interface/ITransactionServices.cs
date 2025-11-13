using AppModels.Models.Search;
using AppModels.Models.Transaction;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface
{
    public interface ITransactionServices
    {
        // Create
        Task<Guid> AddAsync(CreateTransactionDto entity);
        Task<IEnumerable<TransactionDto?>> SearchAsync(TxnSearchDto searchDto);
        Task<AllTransByMerchantDto> GetAllByMerchantIdAsync(Guid merchantId);

        // Read (Get by Id)
        Task<TransactionDto?> GetByIdAsync(Guid id);

        // Read (Get by Id)
        Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<TransactionDto>> GetAllAsync(PaginationEntity param);

        // Update
        Task<bool> UpdateAsync(CreateTransactionDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);
    }
}
