using AppModels.Models;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface
{
    public interface IWarehouseServices
    {
        // Create
        Task<Guid> AddAsync(WarehouseDto entity);

        // Read (Get by Id)
        Task<WarehouseDto?> GetByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<WarehouseDto>> GetAllAsync(PaginationEntity param);
        Task<IEnumerable<WarehouseDto>> GetAllAsync();
        // Update
        Task<bool> UpdateAsync(WarehouseDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);
    }
}
