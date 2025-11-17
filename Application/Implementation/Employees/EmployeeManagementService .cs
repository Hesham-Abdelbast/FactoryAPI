using Application.Interface.Employees;
using AppModels.Common;
using AppModels.Entities.Employees;
using AppModels.Models.Employees;
using AutoMapper;
using DAL;
using DAL.Interface;
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
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(PaginationEntity param)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param), "معايير البحث غير موجودة.");

            // افتراضياً نستخدم DbSet<Employee> من الوحدة UnitOfWork أو DbContext
            var query = _unit.Employees.All
                .OrderByDescending(e => e.StartDate); // ترتيب افتراضي حسب تاريخ بداية العمل

            var items = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return items == null ? Enumerable.Empty<EmployeeDto>()
                                 : _mapper.Map<IEnumerable<EmployeeDto>>(items);
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
                .Where(x => !x.IsDeleted)
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

        public async Task<PagedResult<EmployeeCashAdvanceDto>> GetCashAdvancesAsync(Guid employeeId, PaginationEntity param)
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

            return new PagedResult<EmployeeCashAdvanceDto>
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

        public async Task<IEnumerable<EmployeePersonalExpenseDto>> GetPersonalExpensesAsync(Guid employeeId, PaginationEntity param)
        {
            var query = _unit.EmployeePersonalExpense.All
                .Where(x => x.EmployeeId == employeeId)
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate);

            var list = await query
                .Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeePersonalExpenseDto>>(list);
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

        #region 📅 Payroll

        public async Task<Guid> GeneratePayrollAsync(Guid employeeId, int year, int month)
            => await ExecuteTransactionAsync(async () =>
            {
                var employee = await _unit.Employees.FindAsync(employeeId)
                    ?? throw new Exception("⚠ الموظف غير موجود.");

                var exists = await _unit.EmployeeMonthlyPayroll.All
                    .AnyAsync(x => x.EmployeeId == employeeId && x.Year == year && x.Month == month && !x.IsDeleted);

                if (exists)
                    throw new Exception($"⚠ كشف راتب {month}/{year} موجود بالفعل.");

                var expenses = await _unit.EmployeePersonalExpense.All
                    .Where(x => x.EmployeeId == employeeId && !x.IsDeleted && x.CreateDate.Year == year && x.CreateDate.Month == month)
                    .SumAsync(x => x.Amount);

                var advances = await _unit.EmployeeCashAdvance.All
                    .Where(x => x.EmployeeId == employeeId && !x.IsDeleted && x.CreateDate.Year == year && x.CreateDate.Month == month)
                    .SumAsync(x => x.Amount);

                var payroll = new EmployeeMonthlyPayroll
                {
                    EmployeeId = employeeId,
                    Year = year,
                    Month = month,
                    BaseSalary = employee.BaseSalary,
                    CashAdvancesTotal = advances,
                    PersonalExpensesTotal = expenses
                };

                await _unit.EmployeeMonthlyPayroll.InsertAsync(payroll);
                return payroll.Id;
            });

        public async Task<EmployeeMonthlyPayrollDto?> GetPayrollAsync(Guid employeeId, int year, int month)
        {
            var data = await _unit.EmployeeMonthlyPayroll.All
                .Where(x => !x.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Year == year && x.Month == month);

            return _mapper.Map<EmployeeMonthlyPayrollDto>(data);
        }

        public async Task<IEnumerable<EmployeeMonthlyPayrollDto>> GetPayrollHistoryAsync(Guid employeeId)
        {
            var data = await _unit.EmployeeMonthlyPayroll.All
                .Where(x => x.EmployeeId == employeeId && !x.IsDeleted)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeMonthlyPayrollDto>>(data);
        }

        #endregion

        #region 📊 Reporting

        public async Task<EmployeeFinancialReportDto> GetEmployeeFinancialSummaryAsync(Guid employeeId, DateTime? from = null, DateTime? to = null)
        {
            var cash = _unit.EmployeeCashAdvance.All.Where(x => x.EmployeeId == employeeId && !x.IsDeleted);
            var expenses = _unit.EmployeePersonalExpense.All.Where(x => x.EmployeeId == employeeId && !x.IsDeleted);

            if (from.HasValue && to.HasValue)
            {
                cash = cash.Where(x => x.CreateDate >= from && x.CreateDate <= to);
                expenses = expenses.Where(x => x.CreateDate >= from && x.CreateDate <= to);
            }

            return new EmployeeFinancialReportDto
            {
                EmployeeId = employeeId,
                TotalCashAdvances = await cash.SumAsync(x => x.Amount),
                TotalPersonalExpenses = await expenses.SumAsync(x => x.Amount)
            };
        }

        #endregion
    }
}
