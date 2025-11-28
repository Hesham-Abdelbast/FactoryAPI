using AppModels.Common;
using AppModels.Models.Drivers;
using System.Collections;

namespace Application.Interface.Drivers
{
    public interface IDriverService
    {
        Task<PagedResult<IEnumerable<DriverDto>>> GetAllAsync();

        Task<Guid> AddAsync(CreateDriverDto entity);

        Task<Boolean> UpdateAsync(CreateDriverDto entity);

        Task<Boolean> DeleteAsync(Guid id);

    }
}
