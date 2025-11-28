using Application.Interface.Drivers;
using AppModels.Common;
using AppModels.Models.Drivers;

namespace Application.Implementation.Drivers
{
    public class DriverService : IDriverService
    {
        public Task<Guid> AddAsync(CreateDriverDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<IEnumerable<DriverDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CreateDriverDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
