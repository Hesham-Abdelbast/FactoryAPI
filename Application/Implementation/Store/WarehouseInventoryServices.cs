using Microsoft.EntityFrameworkCore;
using Ejd.GRC.AppModels.Common;
using AppModels.Entities;
using AutoMapper;
using DAL;
using AppModels.Models.Store;
using AppModels.Entities.Store;
using Application.Interface.Store;

namespace Application.Implementation.Store
{
    public class WarehouseInventoryServices : IWarehouseInventoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseInventoryServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 📋 جلب جميع الأصناف داخل المخازن (مع ترقيم الصفحات)
        // ============================================================
        public async Task<IEnumerable<WarehouseInventoryDto>> GetAllAsync(PaginationEntity param)
        {
            var query = _unitOfWork.WarehouseInventory.All
                .Include(x => x.Warehouse)
                .Include(x => x.MaterialType);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WarehouseInventoryDto>>(items);
        }

        // ============================================================
        // 📋 جلب جميع الأصناف داخل المخازن (بدون ترقيم)
        // ============================================================
        public async Task<IEnumerable<WarehouseInventoryDto>> GetAllAsync()
        {
            var items = await _unitOfWork.WarehouseInventory.All
                .Include(x => x.Warehouse)
                .Include(x => x.MaterialType)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WarehouseInventoryDto>>(items);
        }

        // ============================================================
        // 🔎 جلب الأصناف حسب رقم المخزن
        // ============================================================
        public async Task<IEnumerable<WarehouseInventoryDto>> GetByWarehouseIdAsync(Guid warehouseId)
        {
            var items = await _unitOfWork.WarehouseInventory
                .FindByIncluding(x => x.WarehouseId == warehouseId, inc => inc.MaterialType)
                .Include(inc => inc.Warehouse)
                .ToListAsync();

            return _mapper.Map<IEnumerable<WarehouseInventoryDto>>(items);
        }

        // ============================================================
        // 🔎 جلب سجل معين حسب المعرف
        // ============================================================
        public async Task<WarehouseInventoryDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.WarehouseInventory.All
                .Include(x => x.Warehouse)
                .Include(x => x.MaterialType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : _mapper.Map<WarehouseInventoryDto>(entity);
        }

        // ============================================================
        // ➕ إضافة صنف جديد إلى المخزن
        // ============================================================
        public async Task<Guid> AddAsync(WarehouseInventoryDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "⚠ لا يمكن أن تكون بيانات الصنف فارغة.");

                var exists = await _unitOfWork.WarehouseInventory.All
                    .AnyAsync(x => x.MaterialTypeId == dto.MaterialTypeId && x.WarehouseId == dto.WarehouseId);

                if (exists)
                    throw new Exception("⚠ يوجد نفس الصنف داخل هذا المخزن بالفعل.");

                var entity = _mapper.Map<WarehouseInventory>(dto);

                await _unitOfWork.WarehouseInventory.InsertAsync(entity);
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
        // ✏️ تعديل بيانات صنف داخل المخزن
        // ============================================================
        public async Task<bool> UpdateAsync(WarehouseInventoryDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.WarehouseInventory.All
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على الصنف المطلوب.");

                _mapper.Map(dto, entity);

                _unitOfWork.WarehouseInventory.Update(entity);
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
        // 🗑️ حذف صنف من المخزن (Soft Delete)
        // ============================================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.WarehouseInventory.All
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على الصنف المطلوب.");

                entity.IsDeleted = true;

                _unitOfWork.WarehouseInventory.Update(entity);
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
