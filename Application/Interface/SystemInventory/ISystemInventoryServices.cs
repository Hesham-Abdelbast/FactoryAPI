using AppModels.Models.Employees;
using AppModels.Models.MerchantMangement;
using AppModels.Models.SystemInventory;

namespace Application.Interface.SystemInventory
{
    public interface ISystemInventoryServices
    {
        Task<TrnxReportDto> GetTrnxReportAsync(TrnxReportRequestDto trnxDto);
        Task<EmployeeFullFinancialReportDto> GetEmployeeFullFinancialReportAsync(Guid employeeId, DateTime from, DateTime to);
        Task<MerchantInventoryResultDto> GetMerchantInventoryAsync(Guid merchantId, DateTime fromDate, DateTime toDate);
        Task<TrnxReportDto> GetTrnxReportByIdsAsync(List<string> transactionIds);
    }
}
