using Application.Interface.Employees;
using AppModels.Common;
using AppModels.Entities.Employees;
using AppModels.Models.Employees;
using AutoMapper;
using DAL;
using Ejd.GRC.AppModels.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Employees
{
    /// <summary>
    /// Service responsible for managing employee records, payroll,
    /// financial transactions and reports using Unit of Work.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public EmployeeManagementService(IUnitOfWork unit, IMapper mapper)
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

        #region Employee CRUD
        // ============================================================
        // 📋 جلب الموظفين مع التصفية Pagination
        // ============================================================
        public async Task<PagedResult<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync(PaginationEntity param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            var query = _unit.Employees.All
                .OrderByDescending(e => e.StartDate); 

            var totalCount = query.Count();

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<EmployeeDto>>()
            {
                TotalCount = items.Count,
                Data = _mapper.Map<IEnumerable<EmployeeDto>>(items)
            };
        }

        public async Task<Guid> AddEmployeeAsync(EmployeeDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<Employee>(dto);
                await _unit.Employees.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> UpdateEmployeeAsync(EmployeeDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Employees.FindAsync(dto.Id)
                    ?? throw new Exception("⚠ الموظف غير موجود.");

                _mapper.Map(dto, entity);
                _unit.Employees.Update(entity);
                return true;
            });

        public async Task<bool> DeleteEmployeeAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.Employees.FindAsync(id)
                    ?? throw new Exception("⚠ الموظف غير موجود.");

                entity.IsDeleted = true;
                _unit.Employees.Update(entity);
                return true;
            });

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
        {
            var data = await _unit.Employees.All
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .Include(x => x.CashAdvances)
                .Include(x => x.PersonalExpenses)
                .Include(x => x.MonthlyPayrolls)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<EmployeeDto>(data);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var list = await _unit.Employees.All
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(list);
        }

        #endregion

        #region 💰 Cash Advances

        public async Task<Guid> AddCashAdvanceAsync(EmployeeCashAdvanceDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<EmployeeCashAdvance>(dto);
                await _unit.EmployeeCashAdvance.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> DeleteCashAdvanceAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EmployeeCashAdvance.FindAsync(id)
                    ?? throw new Exception("⚠ السلفة غير موجودة.");

                entity.IsDeleted = true;
                _unit.EmployeeCashAdvance.Update(entity);
                return true;
            });

        public async Task<PagedResult<IEnumerable<EmployeeCashAdvanceDto>>> GetCashAdvancesAsync(Guid employeeId, PaginationEntity param)
        {
            var query = _unit.EmployeeCashAdvance.All
                .Where(x => x.EmployeeId == employeeId)
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);

            // Count BEFORE pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var list = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<EmployeeCashAdvanceDto>>
            {
                TotalCount = totalCount,
                Data = _mapper.Map<IEnumerable<EmployeeCashAdvanceDto>>(list)
            };
        }


        public async Task<bool> UpdateEmployeeCashAdvanceAsync(EmployeeCashAdvanceDto dto)
    => await ExecuteTransactionAsync(async () =>
    {
        var entity = await _unit.EmployeeCashAdvance.FindAsync(dto.Id)
            ?? throw new Exception("⚠ السلفة غير موجودة.");

        _mapper.Map(dto, entity);
        entity.UpdateDate = DateTime.UtcNow;

        _unit.EmployeeCashAdvance.Update(entity);
        return true;
    });

        #endregion

        #region 🧾 Personal Expenses

        public async Task<Guid> AddPersonalExpenseAsync(EmployeePersonalExpenseDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = _mapper.Map<EmployeePersonalExpense>(dto);
                await _unit.EmployeePersonalExpense.InsertAsync(entity);
                return entity.Id;
            });

        public async Task<bool> DeletePersonalExpenseAsync(Guid id)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EmployeePersonalExpense.FindAsync(id)
                    ?? throw new Exception("⚠ المصروف غير موجود.");

                entity.IsDeleted = true;
                _unit.EmployeePersonalExpense.Update(entity);
                return true;
            });

        public async Task<PagedResult<IEnumerable<EmployeePersonalExpenseDto>>> GetPersonalExpensesAsync(Guid employeeId, PaginationEntity param)
        {
            var query = _unit.EmployeePersonalExpense.All
                .Where(x => x.EmployeeId == employeeId)
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);

            var totalCount = await query.CountAsync();

            var list = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return new PagedResult<IEnumerable<EmployeePersonalExpenseDto>>()
            {
                Data = _mapper.Map<IEnumerable<EmployeePersonalExpenseDto>>(list),
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdatePersonalExpenseAsync(EmployeePersonalExpenseDto dto)
            => await ExecuteTransactionAsync(async () =>
            {
                var entity = await _unit.EmployeePersonalExpense.FindAsync(dto.Id)
                    ?? throw new Exception("⚠ المصروف غير موجود.");

                // Update allowed fields
                entity.Amount = dto.Amount;
                entity.Note = dto.Note;
                entity.EmployeeId = dto.EmployeeId;

                entity.UpdateDate = DateTime.UtcNow;

                _unit.EmployeePersonalExpense.Update(entity);
                return true;
            });


        #endregion
    }
}
