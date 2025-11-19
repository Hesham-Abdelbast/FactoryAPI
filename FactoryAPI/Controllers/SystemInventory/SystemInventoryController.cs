using Application.Interface.SystemInventory;
using AppModels.Common;
using AppModels.Models.Employees;
using AppModels.Models.SystemInventory;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class SystemInventoryController(ISystemInventoryServices services, ILogger<SystemInventoryController> logger) : ControllerBase
    {
        // ======================================================================
        // 📊 تقرير العمليات بناءً على التاريخ
        // ======================================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<TrnxReportDto>>> GetTrnxReport(DateTime from, DateTime to)
        {
            try
            {
                if (from == default || to == default)
                {
                    return Ok(new TResponse<TrnxReportDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠️ التاريخ مطلوب ولا يمكن أن يكون فارغًا."
                    });
                }

                if (from > to)
                {
                    return Ok(new TResponse<TrnxReportDto>
                    {
                        Success = false,
                        ReturnMsg = "❌ تاريخ البداية لا يمكن أن يكون أكبر من تاريخ النهاية."
                    });
                }

                var result = await services.GetTrnxReportAsync(from, to);

                return Ok(new TResponse<TrnxReportDto>
                {
                    Success = true,
                    ReturnMsg = "📄 تم إنشاء التقرير بنجاح.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetTrnxReport)}");

                return Ok(new TResponse<TrnxReportDto>
                {
                    Success = false,
                    ReturnMsg = " حدث خطأ أثناء إنشاء التقرير: " + ex.Message
                });
            }
        }


        // ======================================================================
        // 📊 تقرير العمليات بناءً على التاريخ
        // ======================================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<EmployeeFullFinancialReportDto>>> GetEmployeeFullFinancialReport(Guid empId,DateTime from, DateTime to)
        {
            try
            {
                if(empId == default)
                {
                    return Ok( new TResponse<EmployeeFullFinancialReportDto>
                    {
                        Success = false,
                        ReturnMsg = "يجب ادخال معرف العامل"
                    });
                }
                if (from == default || to == default)
                {
                    return Ok(new TResponse<EmployeeFullFinancialReportDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠️ التاريخ مطلوب ولا يمكن أن يكون فارغًا."
                    });
                }

                if (from > to)
                {
                    return Ok(new TResponse<EmployeeFullFinancialReportDto>
                    {
                        Success = false,
                        ReturnMsg = "❌ تاريخ البداية لا يمكن أن يكون أكبر من تاريخ النهاية."
                    });
                }

                var result = await services.GetEmployeeFullFinancialReportAsync(empId,from, to);

                return Ok(new TResponse<EmployeeFullFinancialReportDto>
                {
                    Success = true,
                    ReturnMsg = "📄 تم إنشاء التقرير بنجاح.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetTrnxReport)}");

                return Ok(new TResponse<EmployeeFullFinancialReportDto>
                {
                    Success = false,
                    ReturnMsg = " حدث خطأ أثناء إنشاء التقرير: " + ex.Message
                });
            }
        }
    }
}
