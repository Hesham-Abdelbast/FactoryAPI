using Application.Interface.Drivers;
using AppModels.Common;
using AppModels.Entities.Drivers;
using AppModels.Models.Drivers;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Drivers
{
    public class DriverManagementService : IDriverManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DriverManagementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Driver

        public async Task<Guid> AddDriverAsync(CreateDriverDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var driver = _mapper.Map<Driver>(entity);
                await _unitOfWork.Driver.InsertAsync(cancellationToken,driver);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.Commit();
                return driver.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateDriverAsync(CreateDriverDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.Driver.FindAsync(entity.Id);
                if (existing == null) throw new KeyNotFoundException("السائق غير موجود");

                _mapper.Map(entity, existing);
                _unitOfWork.Driver.Update(existing);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteDriverAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.Driver.FindAsync(id);
                if (entity == null) throw new KeyNotFoundException("Driver not found");

                entity.IsDeleted = true;
                _unitOfWork.Driver.Update(entity);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<DriverDto> GetDriverByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Driver.FindAsync(id);
            return _mapper.Map<DriverDto>(entity);
        }

        public async Task<PagedResult<IEnumerable<DriverDto>>> GetAllDriversAsync(PaginationEntity param, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Driver.All.AsNoTracking().OrderBy(x=>x.CreateDate);

            var data = await query
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<IEnumerable<DriverDto>>()
            {
                Data = _mapper.Map<IEnumerable<DriverDto>>(data),
                TotalCount =  query.Count(),
            };
        }

        #endregion


        #region Travel

        public async Task<Guid> AddTravelAsync(CreateTravelDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var model = _mapper.Map<Travel>(entity);
                await _unitOfWork.Travel.InsertAsync(cancellationToken,model);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return model.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateTravelAsync(CreateTravelDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.Travel.FindAsync(entity.Id);
                if (existing == null) throw new KeyNotFoundException("Travel not found");

                _mapper.Map(entity, existing);
                _unitOfWork.Travel.Update(existing);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteTravelAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.Travel.FindAsync(id);
                if (entity == null) throw new KeyNotFoundException("Travel not found");

                entity.IsDeleted = true;
                _unitOfWork.Travel.Update(entity);

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<TravelDto> GetTravelByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Travel.FindAsync(id);
            return _mapper.Map<TravelDto>(entity);
        }

        public async Task<PagedResult<IEnumerable<TravelDto>>> GetAllTravelsAsync(PaginationEntity param, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Travel.All.AsNoTracking().OrderBy(x=>x.CreateDate);

            var result = await query
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<TravelDto>>()
            {
                Data = _mapper.Map<IEnumerable<TravelDto>>(result),
                TotalCount = query.Count(),
            };
        }

        public async Task<PagedResult<IEnumerable<TravelDto>>> GetAllTravelsByDriverIdAsync(Guid driverId,PaginationEntity param, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Travel.All.Where(d => d.DriverId == driverId).OrderBy(x => x.CreateDate);

            var result = await query
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<TravelDto>>()
            {
                Data = _mapper.Map<IEnumerable<TravelDto>>(result),
                TotalCount = query.Count(),
            };
        }
        #endregion


        #region Driver Expense

        public async Task<Guid> AddDriverExpenseAsync(CreateDriverExpenseDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var model = _mapper.Map<DriverExpense>(entity);
                await _unitOfWork.DriverExpense.InsertAsync(cancellationToken,model);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return model.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateDriverExpenseAsync(CreateDriverExpenseDto entity, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.DriverExpense.FindAsync(entity.Id);
                if (existing == null) throw new KeyNotFoundException("Driver expense not found");

                _mapper.Map(entity, existing);
                _unitOfWork.DriverExpense.Update(existing);

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteDriverExpenseAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.DriverExpense.FindAsync(id);
                if (existing == null) throw new KeyNotFoundException("Expense not found");

                existing.IsDeleted = true;
                _unitOfWork.DriverExpense.Update(existing);

                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<DriverExpenseDto> GetDriverExpenseByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.DriverExpense.FindAsync(id);
            return _mapper.Map<DriverExpenseDto>(entity);
        }

        public async Task<PagedResult<IEnumerable<DriverExpenseDto>>> GetAllDriverExpensesAsync(PaginationEntity param, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.DriverExpense.All.AsNoTracking().Where(x => !x.IsDeleted);

            var result = await query
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<DriverExpenseDto>>()
            {
                Data = _mapper.Map<IEnumerable<DriverExpenseDto>>(result),
                TotalCount =query.Count()
            };
        }

        public async Task<PagedResult<IEnumerable<DriverExpenseDto>>> GetAllDriverExpensesByDriverIdAsync(Guid driverId,PaginationEntity param, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.DriverExpense.All.AsNoTracking().Where(x => x.DriverId == driverId);

            var result = await query
                .Skip(param.PageSize * (param.PageIndex - 1))
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<DriverExpenseDto>>()
            {
                Data = _mapper.Map<IEnumerable<DriverExpenseDto>>(result),
                TotalCount = query.Count()
            };
        }

        #endregion
    }
}