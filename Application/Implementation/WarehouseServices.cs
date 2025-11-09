using Microsoft.EntityFrameworkCore;
using Ejd.GRC.AppModels.Common;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Application.Interface;

namespace Application.Implementation
{
    public class WarehouseServices : IWarehouseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 📋 جلب كل المخازن
        // ============================================================
        public async Task<IEnumerable<WarehouseDto>> GetAllAsync(PaginationEntity param)
        {
            var query = _unitOfWork.Warehouse.All;

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            var data = _mapper.Map<IEnumerable<WarehouseDto>>(items);
            return data;
        }

        // ============================================================
        // 🔎 جلب مخزن حسب رقم المعرف
        // ============================================================
        public async Task<WarehouseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Warehouse.All
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null
                ? null
                : _mapper.Map<WarehouseDto>(entity);
        }

        // ============================================================
        // ➕ إضافة مخزن جديد
        // ============================================================
        public async Task<Guid> AddAsync(WarehouseDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "لا يمكن أن تكون بيانات المخزن فارغة.");

                var exists = await _unitOfWork.Warehouse.All
                    .AnyAsync(x => x.Name == dto.Name);

                if (exists)
                    throw new Exception("⚠ يوجد مخزن آخر بنفس الاسم.");

                var entity = _mapper.Map<Warehouse>(dto);

                await _unitOfWork.Warehouse.InsertAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.Commit();
                return entity.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        // ============================================================
        // ✏️ تعديل بيانات مخزن
        // ============================================================
        public async Task<bool> UpdateAsync(WarehouseDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.Warehouse.All
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على المخزن المطلوب.");

                _mapper.Map(dto, entity);

                _unitOfWork.Warehouse.Update(entity);
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

        // ============================================================
        // 🗑️ حذف مخزن (Soft Delete)
        // ============================================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.Warehouse.All
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على المخزن المطلوب.");

                entity.IsDeleted = true;

                _unitOfWork.Warehouse.Update(entity);
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
    }
}
