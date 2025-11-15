using AppModels.Models.Store;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface.Store
{
    public interface IWarehouseInventoryServices
    {
        // Create
        Task<Guid> AddAsync(WarehouseInventoryDto entity);

        // Read (Get by Id)
        Task<WarehouseInventoryDto?> GetByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<WarehouseInventoryDto>> GetAllAsync(PaginationEntity param);
        Task<IEnumerable<WarehouseInventoryDto>> GetAllAsync();
        Task<IEnumerable<WarehouseInventoryDto>> GetByWarehouseIdAsync(Guid warehouseId);
        // Update
        Task<bool> UpdateAsync(WarehouseInventoryDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);
    }
}
