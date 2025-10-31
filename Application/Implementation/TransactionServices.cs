using Application.Interface;
using AppModels.Models;
using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using AppModels.Entities;
using AppModels.Common;

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
        // 📥 إضافة معاملة جديدة + تعديل الكمية في المخزون
        // ============================================================
        public async Task<Guid> AddAsync(TransactionDto entity)
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
        public async Task<bool> UpdateAsync(TransactionDto entity)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "لا يمكن أن تكون المعاملة فارغة.");

                var existing = await _unitOfWork.Transaction.FindAsync(entity.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"المعاملة بالرقم {entity.Id} غير موجودة.");

                // تعديل المخزون إذا تغير النوع أو الكمية
                if ((int)existing.Type !=entity.Type || existing.Quantity != entity.Quantity)
                {
                    await AdjustInventoryAsync(existing, entity);
                }

                _mapper.Map(entity, existing);
                // تعيين معرف فريد للمعاملة
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
        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Transaction.All.ToListAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(entities);
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
        // 🔧 تعديل المخزون بعد إضافة معاملة جديدة
        // ============================================================
        private async Task UpdateInventoryOnAddAsync(Transaction transaction)
        {
            var inventory = await _unitOfWork.StoreInventory
                .FindBy(x => x.MaterialTypeId == transaction.MaterialTypeId, false)
                .FirstOrDefaultAsync();

            if (transaction.Type == AppModels.Common.TransactionType.Income)
            {
                if (inventory != null)
                {
                    inventory.CurrentQuantity += transaction.Quantity;
                    _unitOfWork.StoreInventory.Update(inventory);
                }
                else
                {
                    inventory = new StoreInventory
                    {
                        MaterialTypeId = transaction.MaterialTypeId,
                        CurrentQuantity = transaction.Quantity
                    };
                    await _unitOfWork.StoreInventory.InsertAsync(inventory);
                }
            }
            else if (transaction.Type == AppModels.Common.TransactionType.Outcome)
            {
                if (inventory == null || inventory.CurrentQuantity < transaction.Quantity)
                    throw new Exception("لا توجد كمية كافية في المخزون لتنفيذ عملية الصرف.");

                inventory.CurrentQuantity -= transaction.Quantity;
                _unitOfWork.StoreInventory.Update(inventory);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        // ============================================================
        // 🔁 تعديل المخزون عند تحديث معاملة
        // ============================================================
        private async Task AdjustInventoryAsync(Transaction oldTrans, TransactionDto newTrans)
        {
            var inventory = await _unitOfWork.StoreInventory
                .FindBy(x => x.MaterialTypeId == newTrans.MaterialTypeId, false)
                .FirstOrDefaultAsync();

            if (inventory == null)
                throw new Exception("لا يوجد سجل مخزون لهذا النوع.");

            // إرجاع تأثير المعاملة القديمة
            if (oldTrans.Type == AppModels.Common.TransactionType.Income)
                inventory.CurrentQuantity -= oldTrans.Quantity;
            else if (oldTrans.Type == AppModels.Common.TransactionType.Outcome)
                inventory.CurrentQuantity += oldTrans.Quantity;

            // تطبيق المعاملة الجديدة
            if (newTrans.Type == (int)TransactionType.Income)
                inventory.CurrentQuantity += newTrans.Quantity;
            else if (newTrans.Type == (int)TransactionType.Outcome)
            {
                if (inventory.CurrentQuantity < newTrans.Quantity)
                    throw new Exception("الكمية الجديدة غير متوفرة في المخزون.");
                inventory.CurrentQuantity -= newTrans.Quantity;
            }

            _unitOfWork.StoreInventory.Update(inventory);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
