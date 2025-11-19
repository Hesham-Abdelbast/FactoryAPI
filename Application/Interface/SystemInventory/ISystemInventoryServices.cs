using AppModels.Models.Employees;
using AppModels.Models.SystemInventory;

namespace Application.Interface.SystemInventory
{
    public interface ISystemInventoryServices
    {
        Task<TrnxReportDto> GetTrnxReportAsync(DateTime from, DateTime to);
        Task<EmployeeFullFinancialReportDto> GetEmployeeFullFinancialReportAsync(Guid employeeId, DateTime from, DateTime to);
    }
}
