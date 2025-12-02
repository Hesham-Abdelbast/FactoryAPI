using AppModels.Common;
using AppModels.Models.Drivers;
using Ejd.GRC.AppModels.Common;

namespace Application.Interface.Drivers
{
    public interface IDriverManagementService
    {
        #region Driver

        Task<PagedResult<IEnumerable<DriverDto>>> GetAllDriversAsync(PaginationEntity param, CancellationToken cancellationToken = default);

        Task<DriverDto> GetDriverByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Guid> AddDriverAsync(CreateDriverDto entity, CancellationToken cancellationToken = default);

        Task<bool> UpdateDriverAsync(CreateDriverDto entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteDriverAsync(Guid id, CancellationToken cancellationToken = default);

        #endregion

        #region Travel

        Task<PagedResult<IEnumerable<TravelDto>>> GetAllTravelsAsync(PaginationEntity param, CancellationToken cancellationToken = default);
        Task<PagedResult<IEnumerable<TravelDto>>> GetAllTravelsByDriverIdAsync(Guid driverId, PaginationEntity param, CancellationToken cancellationToken = default);
        Task<TravelDto> GetTravelByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Guid> AddTravelAsync(CreateTravelDto entity, CancellationToken cancellationToken = default);

        Task<bool> UpdateTravelAsync(CreateTravelDto entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteTravelAsync(Guid id, CancellationToken cancellationToken = default);

        #endregion

        #region DriverExpense

        Task<PagedResult<IEnumerable<DriverExpenseDto>>> GetAllDriverExpensesAsync(PaginationEntity param, CancellationToken cancellationToken = default);
        Task<PagedResult<IEnumerable<DriverExpenseDto>>> GetAllDriverExpensesByDriverIdAsync(Guid driverId, PaginationEntity param, CancellationToken cancellationToken = default);
        Task<DriverExpenseDto> GetDriverExpenseByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Guid> AddDriverExpenseAsync(CreateDriverExpenseDto entity, CancellationToken cancellationToken = default);

        Task<bool> UpdateDriverExpenseAsync(CreateDriverExpenseDto entity, CancellationToken cancellationToken = default);

        Task<bool> DeleteDriverExpenseAsync(Guid id, CancellationToken cancellationToken = default);

        #endregion
    }
}