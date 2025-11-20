using Application.Interface.MerchantMangement;
using AppModels.Common;
using AppModels.Common.Enums;
using AppModels.Entities.MerchantMangement;
using AppModels.Models.MerchantMangement;
using AutoMapper;
using DAL;
using DAL.Migrations;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.MerchantMangement
{
    public class MerchantExpenseService : IMerchantExpenseService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public MerchantExpenseService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit ?? throw new ArgumentNullException(nameof(unit), "وحدة العمل غير موجودة.");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "الـ Mapper غير موجود.");
        }

        public async Task<IEnumerable<MerchantExpenseDto>> GetAllAsync(PaginationEntity param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.MerchantExpense.All
                .Include(x => x.Merchant)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.ExpenseDate);

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return items == null ? Enumerable.Empty<MerchantExpenseDto>()
                                 : _mapper.Map<IEnumerable<MerchantExpenseDto>>(items);
        }

        public async Task<PagedResult<MerchantExpenseDto>> GetAllByMerchantIdAsync(Guid merchantId,PaginationEntity param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.MerchantExpense.All
                .Include(x => x.Merchant)
                .Where (x => x.MerchantId == merchantId)
                .OrderByDescending(x => x.ExpenseDate);


            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();
            return new PagedResult<MerchantExpenseDto>()
            {
                Data = items == null ? Enumerable.Empty<MerchantExpenseDto>()
                                 : _mapper.Map<IEnumerable<MerchantExpenseDto>>(items),
                TotalCount = query.Count()
            };
        }

        public async Task<IEnumerable<MerchantExpenseDto>> GetAllAsync()
        {
            var list = await _unit.MerchantExpense.All
                .Include(x => x.Merchant)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.ExpenseDate)
                .ToListAsync();

            return list == null ? Enumerable.Empty<MerchantExpenseDto>()
                                : _mapper.Map<IEnumerable<MerchantExpenseDto>>(list);
        }

        public async Task<MerchantExpenseDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;

            var entity = await _unit.MerchantExpense.All
                .Include(x => x.Merchant)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            return entity == null ? null : _mapper.Map<MerchantExpenseDto>(entity);
        }

        public async Task<Guid> CreateAsync(MerchantExpenseCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto), "بيانات المصاريف غير موجودة.");

            var entity = _mapper.Map<MerchantExpense>(dto);
            if (entity == null) throw new InvalidOperationException("حدث خطأ أثناء تحويل البيانات.");

            entity.ExpenseDate = dto.ExpenseDate ?? DateTime.UtcNow;

            await _unit.MerchantExpense.InsertAsync(entity);
            var saved = await _unit.SaveChangesAsync();
            if (saved <= 0) throw new InvalidOperationException("فشل في حفظ المصاريف.");

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(MerchantExpenseDto dto)
        {
            if (dto == null || dto.Id == Guid.Empty) return false;

            var existing = await _unit.MerchantExpense.FindAsync(dto.Id);
            if (existing == null || existing.IsDeleted) return false;

            _mapper.Map(dto, existing);
            _unit.MerchantExpense.Update(existing);

            return await _unit.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var entity = await _unit.MerchantExpense.FindAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.IsDeleted = true;
            _unit.MerchantExpense.Update(entity);

            return await _unit.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetMerchantExpenseSummaryAsync(Guid merchantId, ExpenseSummaryRequest request)
        {
            if (merchantId == Guid.Empty)
                throw new ArgumentException("معرف التاجر غير صالح.");

            if (request == null)
                throw new ArgumentNullException(nameof(request), "بيانات البحث غير موجودة.");

            // جلب البيانات الأساسية
            var query = _unit.MerchantExpense.All
                .Where(x => x.MerchantId == merchantId);

            // تطبيق الفلترة حسب نوع الطلب
            switch (request.Type)
            {
                case ExpenseSummaryType.Daily:
                    if (request.Date == null)
                        throw new ArgumentNullException(nameof(request.Date), "يجب تحديد التاريخ للبحث اليومي.");

                    var selectedDay = request.Date.Value.Date;

                    query = query.Where(x => x.ExpenseDate.Date == selectedDay);
                    break;

                case ExpenseSummaryType.Monthly:
                    if (request.Date == null)
                        throw new ArgumentNullException(nameof(request.Date), "يجب تحديد الشهر للبحث الشهري.");

                    var selectedMonth = request.Date.Value.Month;
                    var selectedYear = request.Date.Value.Year;

                    query = query.Where(x => x.ExpenseDate.Month == selectedMonth && x.ExpenseDate.Year == selectedYear);
                    break;

                case ExpenseSummaryType.Range:
                    if (request.From == null || request.To == null)
                        throw new ArgumentNullException(nameof(request), "يجب تحديد فترة البداية والنهاية.");

                    var from = request.From.Value.Date;
                    var to = request.To.Value.Date;

                    query = query.Where(x => x.ExpenseDate.Date >= from && x.ExpenseDate.Date <= to);
                    break;

                default:
                    throw new InvalidOperationException("نوع البحث غير معروف.");
            }

            // حساب مجموع السلف للتاجر
            var total = await query.SumAsync(x => x.Amount);

            return total;
        }

    }
}
