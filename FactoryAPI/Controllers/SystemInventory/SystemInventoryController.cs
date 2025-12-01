using Application.Interface.SystemInventory;
using AppModels.Common;
using AppModels.Models.Employees;
using AppModels.Models.MerchantMangement;
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<TrnxReportDto>>> GetTrnxReport([FromBody] TrnxReportRequestDto trnxDto)
        {
            try
            {
                if (trnxDto.From == default || trnxDto.To == default)
                {
                    return Ok(new TResponse<TrnxReportDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠️ التاريخ مطلوب ولا يمكن أن يكون فارغًا."
                    });
                }

                if (trnxDto.From > trnxDto.To)
                {
                    return Ok(new TResponse<TrnxReportDto>
                    {
                        Success = false,
                        ReturnMsg = "❌ تاريخ البداية لا يمكن أن يكون أكبر من تاريخ النهاية."
                    });
                }

                var result = await services.GetTrnxReportAsync(trnxDto);

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

        // ======================================================================
        // 📊 تقرير المخزون والمعاملات للتاجر بناءً على التاريخ
        // ======================================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<MerchantInventoryResultDto>>> GetMerchantInventory(Guid merchantId, DateTime from, DateTime to)
        {
            try
            {
                if (merchantId == Guid.Empty)
                {
                    return Ok(new TResponse<MerchantInventoryResultDto>
                    {
                        Success = false,
                        ReturnMsg = "يجب ادخال معرف التاجر."
                    });
                }

                if (from == default || to == default)
                {
                    return Ok(new TResponse<MerchantInventoryResultDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠️ التاريخ مطلوب ولا يمكن أن يكون فارغًا."
                    });
                }

                if (from > to)
                {
                    return Ok(new TResponse<MerchantInventoryResultDto>
                    {
                        Success = false,
                        ReturnMsg = "❌ تاريخ البداية لا يمكن أن يكون أكبر من تاريخ النهاية."
                    });
                }

                var result = await services.GetMerchantInventoryAsync(merchantId, from, to);

                return Ok(new TResponse<MerchantInventoryResultDto>
                {
                    Success = true,
                    ReturnMsg = "📄 تم إنشاء التقرير بنجاح.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                    logger.LogError(ex, $"{GetType().Name}.{nameof(GetMerchantInventory)}");

                return Ok(new TResponse<MerchantInventoryResultDto>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء إنشاء التقرير: " + ex.Message
                });
            }
        }
    

        // ======================================================================
        // 📦 تقرير العمليات بناءً على قائمة معرفات المعاملات (Ids)
        // ======================================================================
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<TrnxReportDto>>> GetTrnxReportByIds([FromBody] List<string> transactionIds)
        {
            try
            {
                // ⚠ التحقق من أن القائمة ليست فارغة أو null
                if (transactionIds == null || transactionIds.Count == 0)
                {
                    return Ok(new TResponse<TrnxReportDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠️ يجب اختيار عملية واحدة على الأقل لإنشاء التقرير."
                    });
                }

                // 🚀 تنفيذ التقرير
                var result = await services.GetTrnxReportByIdsAsync(transactionIds);

                return Ok(new TResponse<TrnxReportDto>
                {
                    Success = true,
                    ReturnMsg = "📄 تم إنشاء التقرير بنجاح بناءً على المعاملات المحددة.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetTrnxReportByIds)}");

                return Ok(new TResponse<TrnxReportDto>
                {
                    Success = false,
                    ReturnMsg = "❌ حدث خطأ أثناء إنشاء التقرير: " + ex.Message
                });
            }
        }

    }
}
