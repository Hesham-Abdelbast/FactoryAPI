using AppModels.Common;
using AppModels.Models.Employees;
using Ejd.GRC.AppModels.Common;
namespace Application.Interface.Employees
{
    /// <summary>
    /// Service responsible for managing all employee-related operations including:
    /// - Employee CRUD
    /// - Cash advances
    /// - Personal expenses
    /// - Monthly payroll calculation
    /// - Employee financial reports
    /// </summary>
    public interface IEmployeeManagementService
    {
        // Employee CRUD

        Task<Guid> AddEmployeeAsync(EmployeeDto dto);
        Task<bool> UpdateEmployeeAsync(EmployeeDto dto);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<PagedResult<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync(PaginationEntity param);

        // Cash advance operations
        Task<Guid> AddCashAdvanceAsync(EmployeeCashAdvanceDto dto);
        Task<bool> DeleteCashAdvanceAsync(Guid id);
        Task<PagedResult<IEnumerable<EmployeeCashAdvanceDto>>> GetCashAdvancesAsync(Guid employeeId, PaginationEntity param);
        Task<bool> UpdateEmployeeCashAdvanceAsync(EmployeeCashAdvanceDto dto);

        // Personal expense operations
        Task<Guid> AddPersonalExpenseAsync(EmployeePersonalExpenseDto dto);
        Task<bool> DeletePersonalExpenseAsync(Guid id);
        Task<PagedResult<IEnumerable<EmployeePersonalExpenseDto>>> GetPersonalExpensesAsync(Guid employeeId, PaginationEntity param);
        Task<bool> UpdatePersonalExpenseAsync(EmployeePersonalExpenseDto dto);
    }
}
