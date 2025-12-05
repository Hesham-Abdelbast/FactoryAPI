using AppModels.Common;
using AppModels.Models.Search;
using AppModels.Models.Transaction;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface
{
    public interface ITransactionServices
    {
        // Create
        Task<Guid> AddAsync(CreateTransactionDto entity);
        Task<PagedResult<IEnumerable<TransactionDto>?>> SearchAsync(TxnSearchDto searchDto);
        Task<PagedResult<AllTransByMerchantDto>> GetAllByMerchantIdAsync(Guid merchantId, PaginationEntity param);

        // Read (Get by Id)
        Task<TransactionDto?> GetByIdAsync(Guid id);

        // Read (Get by Id)
        Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id);
        Task<InvoiceLstDto> GetInvoiceByIdsAsync(List<Guid> ids);

        // Read (Get all)
        Task<PagedResult<IEnumerable<TransactionDto>>> GetAllAsync(PaginationEntity param);

        // Update
        Task<bool> UpdateAsync(CreateTransactionDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);
    }
}
