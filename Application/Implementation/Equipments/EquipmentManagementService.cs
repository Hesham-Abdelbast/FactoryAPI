using Application.Interface.Equipments;
using AppModels.Common;
using AppModels.Common.Enums;
using AppModels.Entities.Equipments;
using AppModels.Models.Equipments;
using AutoMapper;
using DAL;
using DAL.Migrations;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Equipments
{
    /// <summary>
    /// Service responsible for managing equipments, their incomes and expenses
    /// using Unit of Work pattern and soft delete approach.
    /// </summary>
    public class EquipmentManagementService : IEquipmentManagementService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public EquipmentManagementService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        #region 🔧 Internal Helpers

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

        #region Equipment CRUD
        // ============================================================
        // 📋 جلب المعدات مع التصفية Pagination
        // ============================================================
        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync(PaginationEntity param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.Equipments.All
                .OrderByDescending(e => e.CreateDate);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return items == null ? Enumerable.Empty<EquipmentDto>()
                                 : _mapper.Map<IEnumerable<EquipmentDto>>(items);
        }

        public async Task<Guid> AddEquipmentAsync(EquipmentDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<Equipment>(dto);
                await _unit.Equipments.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> UpdateEquipmentAsync(EquipmentDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Equipments.FindAsync(dto.Id)
                    ?? throw new Exception("⚠ المعدة غير موجودة.");

                _mapper.Map(dto, entity);
                _unit.Equipments.Update(entity);
                return true;
            });

        public async Task<bool> DeleteEquipmentAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Equipments.FindAsync(id)
                    ?? throw new Exception("⚠ المعدة غير موجودة.");

                entity.IsDeleted = true;
                _unit.Equipments.Update(entity);
                return true;
            });

        public async Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id)
        {
            var data = await _unit.Equipments.All
                .Where(x => !x.IsDeleted)
                .Include(x => x.Expenses)
                .Include(x => x.Incomes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<EquipmentDto>(data);
        }

        public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync()
        {
            var list = await _unit.Equipments.All
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<EquipmentDto>>(list);
        }

        #endregion

        #region Equipment Expenses

        public async Task<Guid> AddEquipmentExpenseAsync(EquipmentExpenseDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<EquipmentExpense>(dto);
                await _unit.EquipmentExpense.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> DeleteEquipmentExpenseAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EquipmentExpense.FindAsync(id)
                    ?? throw new Exception("⚠ المصروف غير موجود.");

                entity.IsDeleted = true;
                _unit.EquipmentExpense.Update(entity);
                return true;
            });

        public async Task<IEnumerable<EquipmentExpenseDto>> GetEquipmentExpensesAsync(Guid equipmentId, PaginationEntity param)
        {
            var query = _unit.EquipmentExpense.All
                .Where(x => x.EquipmentId == equipmentId && !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);

            var list = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EquipmentExpenseDto>>(list);
        }

        public async Task<bool> UpdateEquipmentExpenseAsync(EquipmentExpenseDto dto)
                => await ExecuteTransactionAsync(async () =>
                {
                    var entity = await _unit.EquipmentExpense.FindAsync(dto.Id)
                        ?? throw new Exception("⚠ مصروف المعدة غير موجود.");

                    _mapper.Map(dto, entity);
                    entity.UpdateDate = DateTime.UtcNow;

                    _unit.EquipmentExpense.Update(entity);
                    return true;
                });

        #endregion

        #region Equipment Incomes

        public async Task<Guid> AddEquipmentIncomeAsync(EquipmentIncomeDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<EquipmentIncome>(dto);
                await _unit.EquipmentIncome.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> DeleteEquipmentIncomeAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EquipmentIncome.FindAsync(id)
                    ?? throw new Exception("⚠ الإيراد غير موجود.");

                entity.IsDeleted = true;
                _unit.EquipmentIncome.Update(entity);
                return true;
            });

        public async Task<IEnumerable<EquipmentIncomeDto>> GetEquipmentIncomesAsync(Guid equipmentId, PaginationEntity param)
        {
            var query = _unit.EquipmentIncome.All
                .Where(x => x.EquipmentId == equipmentId && !x.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);

            var list = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EquipmentIncomeDto>>(list);
        }

        public async Task<bool> UpdateEquipmentIncomeAsync(EquipmentIncomeDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EquipmentIncome.FindAsync(dto.Id)
                    ?? throw new Exception("⚠ دخل المعدة غير موجود.");

                if (entity.IsDeleted)
                    throw new Exception("⚠ لا يمكن تعديل سجل محذوف.");

                _mapper.Map(dto, entity);
                entity.UpdateDate = DateTime.UtcNow;

                _unit.EquipmentIncome.Update(entity);
                return true;
            });

        #endregion

        public async Task<EquipmentFinancialSummaryDto> GetEquipmentFinancialSummaryAsync(Guid equipmentId, ExpenseSummaryRequest request)
        {
            var equipment = await _unit.Equipments.All
                .Where(x => x.Id == equipmentId)
                .FirstOrDefaultAsync();

            if (equipment == null)
                throw new Exception("⚠ المعدة غير موجودة.");

            // إعداد Query للمصروفات والإيرادات
            var expensesQuery = _unit.EquipmentExpense.All.Where(x => x.EquipmentId == equipmentId);
            var incomesQuery = _unit.EquipmentIncome.All.Where(x => x.EquipmentId == equipmentId);

            if (request.Type == ExpenseSummaryType.Daily && request.Date.HasValue)
            {
                var date = request.Date.Value.Date;
                expensesQuery = expensesQuery.Where(x => x.CreateDate.Date == date);
                incomesQuery = incomesQuery.Where(x => x.CreateDate.Date == date);
            }
            else if (request.Type == ExpenseSummaryType.Monthly && request.Date.HasValue)
            {
                var year = request.Date.Value.Year;
                var month = request.Date.Value.Month;
                expensesQuery = expensesQuery.Where(x => x.CreateDate.Year == year && x.CreateDate.Month == month);
                incomesQuery = incomesQuery.Where(x => x.CreateDate.Year == year && x.CreateDate.Month == month);
            }
            else if (request.Type == ExpenseSummaryType.Range && request.From.HasValue && request.To.HasValue)
            {
                var from = request.From.Value.Date;
                var to = request.To.Value.Date;
                expensesQuery = expensesQuery.Where(x => x.CreateDate.Date >= from && x.CreateDate.Date <= to);
                incomesQuery = incomesQuery.Where(x => x.CreateDate.Date >= from && x.CreateDate.Date <= to);
            }

            var totalExpenses = await expensesQuery.SumAsync(x => x.Amount);
            var totalIncomes = await incomesQuery.SumAsync(x => x.Amount);

            return new EquipmentFinancialSummaryDto
            {
                EquipmentId = equipmentId,
                EquipmentName = equipment.Name,
                TotalExpenses = totalExpenses,
                TotalIncomes = totalIncomes
            };
        }
    }
}
