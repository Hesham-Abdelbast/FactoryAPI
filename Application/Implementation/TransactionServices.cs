using Application.Interface;
using AppModels.Common;
using AppModels.Entities;
using AppModels.Entities.Store;
using AppModels.Models.Search;
using AppModels.Models.Transaction;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // البحث عن معاملة
        // ============================================================
        public async Task<PagedResult<TransactionDto?>> SearchAsync(TxnSearchDto searchDto)
        {
            if (searchDto == null)
                throw new ArgumentNullException(nameof(searchDto), "معايير البحث لا يمكن أن تكون فارغة.");

            // Base query with includes
            var query = _unitOfWork.Transaction
                .All
                .Include(x => x.MaterialType)
                .Include(x => x.Merchant)
                .Include(x => x.Warehouse)
                .AsQueryable();
            var totalCount = await query.CountAsync();
            // ===============================
            // 🔍 Apply dynamic filters
            // ===============================

            if (!string.IsNullOrWhiteSpace(searchDto.MerchantName))
                query = query.Where(x => x.Merchant != null && x.Merchant.Name.Contains(searchDto.MerchantName));

            if (!string.IsNullOrWhiteSpace(searchDto.MaterialTypeName))
                query = query.Where(x => x.MaterialType != null && x.MaterialType.Name.Contains(searchDto.MaterialTypeName));

            if (!string.IsNullOrWhiteSpace(searchDto.WarehouseName))
                query = query.Where(x => x.Warehouse != null && x.Warehouse.Name.Contains(searchDto.WarehouseName));

            if (searchDto.FromDate.HasValue)
                query = query.Where(x => x.CreateDate >= searchDto.FromDate.Value);

            if (searchDto.ToDate.HasValue)
                query = query.Where(x => x.CreateDate <= searchDto.ToDate.Value);

            // ===============================
            // 📄 Pagination
            // ===============================
            int skip = (searchDto.PageIndex - 1) * searchDto.PageSize;
            query = query
                .OrderByDescending(x => x.CreateDate) // Sort by latest transaction
                .Skip(skip)
                .Take(searchDto.PageSize);

            // ===============================
            // 🔁 Map to DTOs
            // ===============================
            var result = await query
                .AsNoTracking()
                .ToListAsync();


            return new PagedResult<TransactionDto?>
            {
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<TransactionDto>>(result)
            };
        }

        // ============================================================
        // 📥 إضافة معاملة جديدة + تعديل الكمية في المخزون
        // ============================================================
        public async Task<Guid> AddAsync(CreateTransactionDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "لا يمكن أن تكون المعاملة فارغة.");

                var transaction = _mapper.Map<Transaction>(entity);

                // تعيين معرف فريد للمعاملة
                transaction.TransactionIdentifier = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid().ToString().Substring(0, 3).ToUpper()}";

                await _unitOfWork.Transaction.InsertAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                await UpdateInventoryOnAddAsync(transaction);

                _unitOfWork.Commit();
                return transaction.Id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        // ============================================================
        // ✏️ تعديل معاملة + تحديث المخزون إذا تغير النوع أو الكمية
        // ============================================================
        public async Task<bool> UpdateAsync(CreateTransactionDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "لا يمكن أن تكون المعاملة فارغة.");

                var existing = await _unitOfWork.Transaction.FindAsync(entity.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"المعاملة غير موجودة.");

                // تعديل المخزون إذا تغير النوع أو الكمية أو المخزن
                if (existing.Type != entity.Type || 
                    existing.Quantity != entity.Quantity || 
                    existing.WarehouseId != entity.WarehouseId || 
                    existing.MaterialTypeId != entity.MaterialTypeId)
                {
                    await AdjustInventoryAsync(existing, entity);
                }

                _mapper.Map(entity, existing);
                // تعيين معرف فريد للمعاملة
                if (string.IsNullOrWhiteSpace(existing.TransactionIdentifier))
                    existing.TransactionIdentifier = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid().ToString().Substring(0, 3).ToUpper()}";
                _unitOfWork.Transaction.Update(existing);
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
        // ❌ حذف معاملة
        // ============================================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var existing = await _unitOfWork.Transaction.FindAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException($"المعاملة بالرقم {id} غير موجودة.");
                existing.IsDeleted = true;

                //Update the store inventory accordingly
                await UpdateInventoryOnDeleteAsync(existing);

                _unitOfWork.Transaction.Update(existing);
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
        // 📋 جلب كل المعاملات
        // ============================================================
        public async Task<PagedResult<TransactionDto>> GetAllAsync(PaginationEntity param)
        {
            var query = _unitOfWork.Transaction.All.Include(x=>x.MaterialType).Include(x => x.Merchant).Include(x=>x.Warehouse);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<TransactionDto>()
            {
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<TransactionDto>>(items)
            };
        }
        // ============================================================
        // 📋 جلب كل المعاملات بناءً على معرف التاجر
        // ============================================================
        public async Task<AllTransByMerchantDto> GetAllByMerchantIdAsync(Guid merchantId)
        {
            var entities = await _unitOfWork.Transaction.All
                .Where(tr => tr.MerchantId == merchantId)
                .Include(x=>x.MaterialType)
                .Include(x => x.Merchant)
                .Include(x => x.Warehouse)
                .ToListAsync();
            var resulat = new AllTransByMerchantDto
            {
                Transactions = _mapper.Map<List<TransactionDto>>(entities),
                TotalMoneyProcessed = entities.Sum(e => e.TotalAmount),
                TotalMoneypay = entities.Sum(e => e.AmountPaid),
                TotalImpurities = entities.Sum(e => e.WeightOfImpurities),
                TotalWight = entities.Sum(e => e.Quantity)
            };
            return resulat;
        }

        // ============================================================
        // 🔍 التأكد من وجود معاملة برقم معين
        // ============================================================
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _unitOfWork.Transaction.FindAsync(id) != null;
        }

        // ============================================================
        // 🔎 جلب معاملة حسب رقمها
        // ============================================================
        public async Task<TransactionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Transaction.FindAsync(id);
            return _mapper.Map<TransactionDto?>(entity);
        }
        // ============================================================
        // 🔎 جلب فاتوره حسب رقمها
        // ============================================================
        public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Transaction.FindByIncluding(x => x.Id == id, inc => inc.MaterialType)
                .Include(inc => inc.Merchant).Include(inc => inc.Warehouse).FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var companyInfo = await _unitOfWork.Contact.All.FirstOrDefaultAsync();

            var  invoiceDto = new InvoiceDto
            {
                TransactionIdentifier = entity.TransactionIdentifier,
                Type = entity.Type,
                MaterialTypeName = entity.MaterialType?.Name,
                CarDriverName = entity.CarDriverName,
                CarAndMatrerialWeight = entity.CarAndMatrerialWeight,
                CarWeight = entity.CarWeight,
                Quantity = entity.Quantity,
                PercentageOfImpurities = entity.PercentageOfImpurities,
                WeightOfImpurities = entity.WeightOfImpurities,
                PricePerUnit = entity.PricePerUnit,
                TotalAmount = entity.TotalAmount,
                MerchantName = entity.Merchant?.Name,
                WarehouseName = entity.Warehouse?.Name,
                Notes = entity.Notes,
                AmountPaid = entity.AmountPaid,
                RemainingAmount = entity.RemainingAmount,
                IsFullyPaid = entity.IsFullyPaid,
                CreateDate = entity.CreateDate,
                ShowPhoneNumber = entity.ShowPhoneNumber,

                CompanyName = companyInfo?.CompanyName ?? "شركه الكواكب",
                CompanyAddress = companyInfo?.Address ?? "",
                CompanyPhone = companyInfo?.Phone ?? ""
            };

            return invoiceDto;
        }


        #region Private Methods


        // ============================================================
        // 🔧 تعديل المخزون بعد إضافة معاملة جديدة
        // ============================================================
        private async Task UpdateInventoryOnAddAsync(Transaction transaction)
        {
            var inventory = await _unitOfWork.WarehouseInventory
                .FindBy(wai => wai.MaterialTypeId == transaction.MaterialTypeId  && wai.WarehouseId == transaction.WarehouseId, false)
                .FirstOrDefaultAsync();

            if (transaction.Type == AppModels.Common.TransactionType.Income)
            {
                if (inventory != null)
                {
                    inventory.CurrentQuantity += transaction.Quantity;
                    _unitOfWork.WarehouseInventory.Update(inventory);
                }
                else
                {
                    inventory = new WarehouseInventory
                    {
                        MaterialTypeId = transaction.MaterialTypeId,
                        WarehouseId = transaction.WarehouseId,
                        CurrentQuantity = transaction.Quantity,
                        Notes = "تم الإنشاء تلقائيًا عند إضافة معاملة دخل."
                    };
                    await _unitOfWork.WarehouseInventory.InsertAsync(inventory);
                }
            }
            else if (transaction.Type == AppModels.Common.TransactionType.Outcome)
            {
                if (inventory == null || inventory.CurrentQuantity < transaction.Quantity)
                    throw new Exception("لا توجد كمية كافية في المخزون لتنفيذ عملية الصرف.");

                inventory.CurrentQuantity -= transaction.Quantity;
                _unitOfWork.WarehouseInventory.Update(inventory);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        // ============================================================
        // 🔧 تعديل المخزون بعد حذف معاملة 
        // ============================================================
        private async Task UpdateInventoryOnDeleteAsync(Transaction transaction)
        {
            var inventory = await _unitOfWork.WarehouseInventory
              .FindBy(wai => wai.MaterialTypeId == transaction.MaterialTypeId && wai.WarehouseId == transaction.WarehouseId, false)
              .FirstOrDefaultAsync();

            if (transaction.Type == AppModels.Common.TransactionType.Income)
            {
                if (inventory == null || inventory.CurrentQuantity < transaction.Quantity)
                    throw new Exception("لا توجد كمية كافية في المخزون لتنفيذ عملية الصرف.");

                if (inventory != null)
                {
                    inventory.CurrentQuantity -= transaction.Quantity;
                    _unitOfWork.WarehouseInventory.Update(inventory);
                }
            }
            else if (transaction.Type == AppModels.Common.TransactionType.Outcome)
            {

                inventory.CurrentQuantity += transaction.Quantity;
                _unitOfWork.WarehouseInventory.Update(inventory);
            }

            await _unitOfWork.SaveChangesAsync();

        }

        // ============================================================
        // 🔁 تعديل المخزون عند تحديث معاملة
        // ============================================================
        private async Task AdjustInventoryAsync(Transaction oldTrans, CreateTransactionDto newTrans)
        {
            // الحصول على سجل المخزون الصحيح
            var inventory = await _unitOfWork.WarehouseInventory
                .FindBy(inv => inv.WarehouseId == newTrans.WarehouseId &&
                               inv.MaterialTypeId == newTrans.MaterialTypeId, false)
                .FirstOrDefaultAsync();

            if (inventory == null)
                throw new Exception("❌ لا يوجد مخزون لهذا الصنف داخل هذا المخزن.");

            // ============================================================
            // 1. إلغاء تأثير العملية السابقة
            // ============================================================
            switch (oldTrans.Type)
            {
                case TransactionType.Income:
                    inventory.CurrentQuantity -= oldTrans.Quantity;
                    break;

                case TransactionType.Outcome:
                    inventory.CurrentQuantity += oldTrans.Quantity;
                    break;

                default:
                    throw new Exception("❌ نوع المعاملة القديمة غير معروف.");
            }

            if (inventory.CurrentQuantity < 0)
                inventory.CurrentQuantity = 0; // حماية إضافية من السالب

            // ============================================================
            // 2. تطبيق تأثير العملية الجديدة
            // ============================================================
            switch (newTrans.Type)
            {
                case TransactionType.Income:
                    inventory.CurrentQuantity += newTrans.Quantity;
                    break;

                case TransactionType.Outcome:
                    if (inventory.CurrentQuantity < newTrans.Quantity)
                        throw new Exception("❌ الكمية غير متوفرة في المخزون.");
                    inventory.CurrentQuantity -= newTrans.Quantity;
                    break;

                default:
                    throw new Exception("❌ نوع المعاملة الجديدة غير معروف.");
            }

            // ============================================================
            // تحديث تاريخ آخر تعديل
            // ============================================================
            inventory.UpdateDate = DateTime.UtcNow;

            _unitOfWork.WarehouseInventory.Update(inventory);
            await _unitOfWork.SaveChangesAsync();
        }
        #endregion

    }
}
