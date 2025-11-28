using Application.Interface;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class FinancingService : IFinancingService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public FinancingService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit), "وحدة العمل غير موجودة.");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "الـ Mapper غير موجود.");
        }

        // ============================================================
        // 📋 جلب البيانات مع التصفية Pagination
        // ============================================================
        public async Task<IEnumerable<FinancingDto>> GetAllAsync(PaginationEntity param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.Financing.All
                .Where(f => !f.IsDeleted)
                .OrderByDescending(x => x.CreateDate);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return items == null ? Enumerable.Empty<FinancingDto>()
                                 : _mapper.Map<IEnumerable<FinancingDto>>(items);
        }

        // ============================================================
        // 📋 إنشاء تمويل جديد
        // ============================================================
        public async Task<Guid> CreateAsync(FinancingCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "بيانات التمويل غير موجودة.");

            var entity = _mapper.Map<Financing>(dto);
            if (entity == null)
                throw new InvalidOperationException("حدث خطأ أثناء تحويل البيانات.");

            await _unit.Financing.InsertAsync(entity);
            var saved = await _unit.SaveChangesAsync();
            if (saved <= 0)
                throw new InvalidOperationException("فشل في حفظ التمويل.");

            return entity.Id;
        }

        // ============================================================
        // 📋 تحديث بيانات التمويل
        // ============================================================
        public async Task<bool> UpdateAsync(FinancingDto dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
                return false;

            var existing = await _unit.Financing.FindAsync(dto.Id);
            if (existing == null || existing.IsDeleted)
                return false;

            _mapper.Map(dto, existing);
            _unit.Financing.Update(existing);

            return await _unit.SaveChangesAsync() > 0;
        }

        // ============================================================
        // 📋 حذف التمويل (حذف ناعم)
        // ============================================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var entity = await _unit.Financing.FindAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.IsDeleted = true;
            _unit.Financing.Update(entity);

            return await _unit.SaveChangesAsync() > 0;
        }

        // ============================================================
        // 📋 جلب التمويل حسب Id
        // ============================================================
        public async Task<FinancingDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;

            var entity = await _unit.Financing.FindAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            return _mapper.Map<FinancingDto>(entity);
        }

        // ============================================================
        // 📋 جلب كل التمويلات بدون Pagination
        // ============================================================
        public async Task<IEnumerable<FinancingDto>> GetAllAsync()
        {
            var list = await _unit.Financing.All
                .Where(f => !f.IsDeleted)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            return list == null ? Enumerable.Empty<FinancingDto>()
                                : _mapper.Map<IEnumerable<FinancingDto>>(list);
        }
    }
}
