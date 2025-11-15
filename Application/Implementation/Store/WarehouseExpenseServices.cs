using Application.Interface.Store;
using AppModels.Common;
using AppModels.Common.Enums;
using AppModels.Entities.Store;
using AppModels.Models.Store;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Store
{
    public class WarehouseExpenseServices : IWarehouseExpenseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseExpenseServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 📋 Get all warehouse expenses (Paginated)
        // ============================================================
        public async Task<IEnumerable<WarehouseExpenseDto>> GetAllAsync(PaginationEntity param)
        {
            var query = _unitOfWork.WarehouseExpense.All;

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize).ToListAsync();

            return _mapper.Map<IEnumerable<WarehouseExpenseDto>>(items);
        }

        // ============================================================
        // 📋 Get all warehouse expenses
        // ============================================================
        public async Task<IEnumerable<WarehouseExpenseDto>> GetAllAsync()
        {
            var items = await _unitOfWork.WarehouseExpense.All.ToListAsync();
            return _mapper.Map<IEnumerable<WarehouseExpenseDto>>(items);
        }

        // ============================================================
        // 🔎 Get expense by Id
        // ============================================================
        public async Task<WarehouseExpenseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.WarehouseExpense.All
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : _mapper.Map<WarehouseExpenseDto>(entity);
        }

        // ============================================================
        // ➕ Add new warehouse expense
        // ============================================================
        public async Task<Guid> AddAsync(WarehouseExpenseDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "بيانات المصروف لا يمكن أن تكون فارغة.");

                var entity = _mapper.Map<WarehouseExpense>(dto);

                await _unitOfWork.WarehouseExpense.InsertAsync(entity);
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
        // ✏️ Update warehouse expense
        // ============================================================
        public async Task<bool> UpdateAsync(WarehouseExpenseDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.WarehouseExpense.All
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على المصروف.");

                _mapper.Map(dto, entity);

                _unitOfWork.WarehouseExpense.Update(entity);
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
        // 🗑️ Delete warehouse expense (Soft Delete)
        // ============================================================
        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = await _unitOfWork.WarehouseExpense.All
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                    throw new KeyNotFoundException("⚠ لم يتم العثور على المصروف.");

                entity.IsDeleted = true;

                _unitOfWork.WarehouseExpense.Update(entity);
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

        public async Task<ExpenseWareSumResponse> GetExpenseSummaryAsync(ExpenseSummaryRequest request)
        {
            var query = _unitOfWork.WarehouseExpense.All.Include(x=>x.Warehouse).AsQueryable();

            switch (request.Type)
            {
                case ExpenseSummaryType.Daily:
                    if (request.Date is null) throw new Exception("Date is required for daily summary.");

                    query = query.Where(x => x.CreateDate.Date == request.Date.Value.Date);
                    break;

                case ExpenseSummaryType.Monthly:
                    if (request.Date is null) throw new Exception("Date is required for monthly summary.");

                    query = query.Where(x =>
                        x.CreateDate.Month == request.Date.Value.Month &&
                        x.CreateDate.Year == request.Date.Value.Year);
                    break;

                case ExpenseSummaryType.Range:
                    if (request.From is null || request.To is null)
                        throw new Exception("From and To dates are required for range.");

                    query = query.Where(x =>
                        x.CreateDate.Date >= request.From.Value.Date &&
                        x.CreateDate.Date <= request.To.Value.Date);
                    break;
            }

            var result = await query.ToListAsync();

            return new ExpenseWareSumResponse
            {
                TotalExpense = result.Sum(x => x.Amount),
                TotalRecords = result.Count,
                Details = _mapper.Map<List<WarehouseExpenseDto>>(result)
            };
        }

    }
}
