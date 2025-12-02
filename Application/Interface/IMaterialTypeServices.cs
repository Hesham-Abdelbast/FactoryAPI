using AppModels.Common;
using AppModels.Entities;
using AppModels.Models;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface
{
    public interface IMaterialTypeServices
    {
        // Create
        Task<Guid> AddAsync(MaterialTypeDto entity);

        // Read (Get by Id)
        Task<MaterialTypeDto?> GetByIdAsync(Guid id);

        // Read (Get all)
        Task<IEnumerable<MaterialTypeDto>> GetAllAsync();
        Task<PagedResult<IEnumerable<MaterialTypeDto>>> GetAllAsync(PaginationEntity param);

        // Update
        Task<bool> UpdateAsync(MaterialTypeDto entity);

        // Delete
        Task<bool> DeleteAsync(Guid id);

        // Optional: Check if exists
        Task<bool> ExistsAsync(Guid id);

    }
}
