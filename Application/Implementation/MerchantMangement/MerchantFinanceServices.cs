using Application.Interface.MerchantMangement;
using AppModels.Common;
using AppModels.Common.Enums;
using AppModels.Entities.MerchantMangement;
using AppModels.Models.MerchantMangement;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.MerchantMangement
{
    public class MerchantFinanceServices : IMerchantFinanceServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public MerchantFinanceServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit), "وحدة العمل غير موجودة.");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "الـ Mapper غير موجود.");
        }

        public async Task<PagedResult< IEnumerable<MerchantFinanceDto>>> GetAllAsync(PaginationEntity param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.MerchantFinance.All
                .Include(x => x.Merchant)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.OperationDate);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();
            return new PagedResult<IEnumerable<MerchantFinanceDto>>()
            {
                Data = items == null ? Enumerable.Empty<MerchantFinanceDto>()
                                 : _mapper.Map<IEnumerable<MerchantFinanceDto>>(items),
                TotalCount = query.Count()
            };
        }

        public async Task<PagedResult<IEnumerable<MerchantFinanceDto>>> GetAllByMerchantIdAsync(Guid merchantId, PaginationEntity param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.MerchantFinance.All
                .Include(x => x.Merchant)
                .Where(x => x.MerchantId == merchantId)
                .OrderByDescending(x => x.OperationDate);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<MerchantFinanceDto>>()
            {
                Data = items == null ? Enumerable.Empty<MerchantFinanceDto>()
                                       : _mapper.Map<IEnumerable<MerchantFinanceDto>>(items),
                TotalCount = query.Count()
            };
        }

        public async Task<IEnumerable<MerchantFinanceDto>> GetAllAsync()
        {
            var list = await _unit.MerchantFinance.All
                .Include(x => x.Merchant)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.OperationDate)
                .ToListAsync();

            return list == null ? Enumerable.Empty<MerchantFinanceDto>()
                                 : _mapper.Map<IEnumerable<MerchantFinanceDto>>(list);
        }

        public async Task<MerchantFinanceDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;

            var entity = await _unit.MerchantFinance.All
                .Include(x => x.Merchant)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return entity == null ? null : _mapper.Map<MerchantFinanceDto>(entity);
        }

        public async Task<Guid> CreateAsync(MerchantFinanceDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), "البيانات غير موجودة.");

            var entity = _mapper.Map<MerchantFinance>(dto);
            if (entity == null) throw new InvalidOperationException("فشل تحويل البيانات.");

            entity.OperationDate = dto.OperationDate == default ? DateTime.UtcNow : dto.OperationDate;

            await _unit.MerchantFinance.InsertAsync(entity);
            var saved = await _unit.SaveChangesAsync();
            if (saved <= 0) throw new InvalidOperationException("فشل في حفظ البيانات.");

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MerchantFinanceDto dto)
        {
            if (dto == null || dto.Id == Guid.Empty) return false;

            var existing = await _unit.MerchantFinance.FindAsync(dto.Id);
            if (existing == null || existing.IsDeleted) return false;

            _mapper.Map(dto, existing);
            _unit.MerchantFinance.Update(existing);

            return await _unit.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var entity = await _unit.MerchantFinance.FindAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.IsDeleted = true;
            _unit.MerchantFinance.Update(entity);

            return await _unit.SaveChangesAsync() > 0;
        }
       
    }
}
