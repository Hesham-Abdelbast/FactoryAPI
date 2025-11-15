using AppModels.Models;
using Ejd.GRC.AppModels.Common;
namespace Application.Interface
{
    public interface IFinancingService
    {
        Task<IEnumerable<FinancingDto>> GetAllAsync(PaginationEntity param);
        Task<Guid> CreateAsync(FinancingCreateDto dto);
        Task<bool> UpdateAsync(FinancingDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<FinancingDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<FinancingDto>> GetAllAsync();
    }
}
