using AppModels.Common;
using AppModels.Models.Store;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface.Store
{
    public interface IWarehouseExpenseServices
    {
        Task<IEnumerable<WarehouseExpenseDto>> GetAllAsync(PaginationEntity param);
        Task<IEnumerable<WarehouseExpenseDto>> GetAllAsync();
        Task<WarehouseExpenseDto?> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(WarehouseExpenseDto dto);
        Task<bool> UpdateAsync(WarehouseExpenseDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ExpenseWareSumResponse> GetExpenseSummaryAsync(ExpenseSummaryRequest request);
    }
}
