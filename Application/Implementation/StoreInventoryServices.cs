using Application.Interface;
using AppModels.Entities;
using AppModels.Models;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation
{
    public class StoreInventoryServices : IStoreInventoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public StoreInventoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ============================================================
        // 📋 جلب كل المواد من المخزون
        // ============================================================
        public async Task<IEnumerable<StoreSummaryDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.StoreInventory.All
                .Include(x => x.MaterialType)
                .Select(x => new StoreSummaryDto
                {
                    MaterialTypeId = x.MaterialTypeId,
                    MaterialTypeName = x.MaterialType.Name,
                    CurrentQuantity = x.CurrentQuantity
                })
                .ToListAsync();

            return entities;
        }

        // ============================================================
        // 🔎 جلب مادة حسب رقم النوع
        // ============================================================
        public async Task<StoreSummaryDto?> GetByMaterialTypeIdAsync(Guid materialTypeId)
        {
            var entity = await _unitOfWork.StoreInventory.All
                .Include(x => x.MaterialType)
                .FirstOrDefaultAsync(x => x.MaterialTypeId == materialTypeId);

            return entity == null ? null : new StoreSummaryDto
            {
                MaterialTypeId = entity.MaterialTypeId,
                MaterialTypeName = entity.MaterialType?.Name ?? string.Empty,
                CurrentQuantity = entity.CurrentQuantity
            };
        }

        // ============================================================
        // ➕ إضافة مادة جديدة للمخزون
        // ============================================================
        public async Task<Guid> AddAsync(StoreSummaryDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "لا يمكن أن تكون بيانات المادة فارغة.");

                var exists = await _unitOfWork.StoreInventory.All
                    .AnyAsync(x => x.MaterialTypeId == dto.MaterialTypeId);

                if (exists)
                    throw new Exception("هذه المادة موجودة بالفعل في المخزون.");

                var entity = new StoreInventory
                {
                    MaterialTypeId = dto.MaterialTypeId,
                    CurrentQuantity = dto.CurrentQuantity
                };

                await _unitOfWork.StoreInventory.InsertAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.Commit();
                return entity.MaterialTypeId;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        // ============================================================
        // ✏️ تعديل بيانات مادة موجودة
        // ============================================================
        public async Task<bool> UpdateAsync(StoreSummaryDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.StoreInventory.All
                    .FirstOrDefaultAsync(x => x.MaterialTypeId == dto.MaterialTypeId);

                if (entity == null)
                    throw new KeyNotFoundException("لم يتم العثور على المادة المطلوبة.");

                entity.CurrentQuantity = dto.CurrentQuantity;
                _unitOfWork.StoreInventory.Update(entity);

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
        // 🗑️ حذف مادة من المخزون
        // ============================================================
        public async Task<bool> DeleteAsync(Guid materialTypeId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.StoreInventory.All
                    .FirstOrDefaultAsync(x => x.MaterialTypeId == materialTypeId);

                if (entity == null)
                    throw new KeyNotFoundException("لم يتم العثور على المادة المطلوبة.");

                entity.IsDeleted = true;
                _unitOfWork.StoreInventory.Update(entity);
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
        // 🔍 التحقق من وجود مادة معينة
        // ============================================================
        public async Task<bool> ExistsAsync(Guid materialTypeId)
        {
            return await _unitOfWork.StoreInventory.All
                .AnyAsync(x => x.MaterialTypeId == materialTypeId);
        }
    }
}
