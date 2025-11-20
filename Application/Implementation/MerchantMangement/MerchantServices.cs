using Application.Interface.MerchantMangement;
using AppModels.Common;
using AppModels.Entities.MerchantMangement;
using AppModels.Models.MerchantMangement;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.MerchantMangement
{
    /// <summary>
    /// ⚙ خدمة مسؤولة عن إدارة التجار، السُلف، والمصاريف مع دعم المعاملات و Pagination
    /// </summary>
    public class MerchantServices : IMerchantServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public MerchantServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region 🔧 Internal Transaction Helpers

        private async Task<T> ExecuteTransactionAsync<T>(Func<Task<T>> action)
        {
            _unit.BeginTransaction();
            try
            {
                var result = await action();
                await _unit.SaveChangesAsync();
                _unit.Commit();
                return result;
            }
            catch
            {
                _unit.Rollback();
                throw;
            }
        }

        private async Task ExecuteTransactionAsync(Func<Task> action)
        {
            _unit.BeginTransaction();
            try
            {
                await action();
                await _unit.SaveChangesAsync();
                _unit.Commit();
            }
            catch
            {
                _unit.Rollback();
                throw;
            }
        }

        #endregion

        #region 🧾 Merchant CRUD

        public async Task<Guid> AddAsync(MerchantDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                if (dto == null) throw new Exception("⚠ لا يمكن إضافة بيانات فارغة.");

                var entity = _mapper.Map<Merchant>(dto);
                await _unit.Merchant.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> UpdateAsync(MerchantDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Merchant.FindAsync(dto.Id)
                    ?? throw new Exception("⚠ التاجر غير موجود.");

                _mapper.Map(dto, entity);
                _unit.Merchant.Update(entity);
                return true;
            });

        public async Task<bool> DeleteAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Merchant.FindAsync(id)
                    ?? throw new Exception("⚠ التاجر غير موجود.");

                entity.IsDeleted = true;
                _unit.Merchant.Update(entity);
                return true;
            });

        public async Task<MerchantDto?> GetByIdAsync(Guid id)
        {
            var data = await _unit.Merchant.All
                .Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();

            return _mapper.Map<MerchantDto>(data);
        }

        public async Task<IEnumerable<MerchantDto>> GetAllAsync()
        {
            var list = await _unit.Merchant.All
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreateDate)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<MerchantDto>>(list);
        }

        public async Task<PagedResult<MerchantDto>> GetAllAsync(PaginationEntity param)
        {
            var query = _unit.Merchant.All;

            if(!string.IsNullOrEmpty(param.Search))
                query = query.Where(x=>x.Name.Contains(param.Search));

            // Get total before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination + ordering + project to DTO at DB level
            var merchants = await query
                .OrderByDescending(x => x.CreateDate)
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .AsNoTracking()
                .ProjectTo<MerchantDto>(_mapper.ConfigurationProvider) // Efficient: projection in DB
                .ToListAsync();

            return new PagedResult<MerchantDto>
            {
                Data = merchants,
                TotalCount = totalCount,
            };
        }

        public async Task<bool> ExistsAsync(Guid id)
            => await _unit.Merchant.All
                    .AnyAsync(x => x.Id == id && !x.IsDeleted);

        #endregion
    }
}
