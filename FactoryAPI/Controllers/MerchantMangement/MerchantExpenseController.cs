using Application.Interface.MerchantMangement;
using AppModels.Common;
using AppModels.Models.MerchantMangement;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FactoryAPI.Controllers.MerchantMangement
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class MerchantExpenseController(IMerchantExpenseService services, ILogger<MerchantExpenseController> logger) : ControllerBase
    {

        // ============================================================
        // 📋 جلب كل السلف بدون Pagination
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<TResponse<List<MerchantExpenseDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<MerchantExpenseDto>>
                {
                    Success = true,
                    Data = result.ToList(),
                    ReturnMsg = "تم جلب جميع السلف بنجاح"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<MerchantExpenseDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        // ============================================================
        // 📋 جلب كل السلف مع Pagination
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<List<MerchantExpenseDto>>>> GetAllWithPagination(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<MerchantExpenseDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع السلف بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllWithPagination)}");
                return Ok(new TResponse<List<MerchantExpenseDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء الجلب: " + ex.Message
                });
            }
        }


        // ============================================================
        // 🔍 جلب سلفة حسب ID
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<MerchantExpenseDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<MerchantExpenseDto> { Success = false, ReturnMsg = "السلفة غير موجودة" });

                return Ok(new TResponse<MerchantExpenseDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<MerchantExpenseDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ➕ إضافة سلفة جديدة
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] MerchantExpenseCreateDto dto)
        {
            try
            {
                var id = await services.CreateAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تم إضافة السلفة بنجاح"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ✏️ تحديث سلفة
        // ============================================================
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] MerchantExpenseDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل السلفة بنجاح" : "فشل التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ❌ حذف سلفة
        // ============================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await services.DeleteAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم حذف السلفة بنجاح" : "فشل الحذف"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // 📊 ملخص السلف لتاجر خلال فترة
        // ============================================================
        [HttpPost("{merchantId:guid}")]
        public async Task<ActionResult<TResponse<decimal>>> GetSummary(Guid merchantId, ExpenseSummaryRequest request)
        {
            try
            {
                var total = await services.GetMerchantExpenseSummaryAsync(merchantId, request);

                return Ok(new TResponse<decimal>
                {
                    Success = true,
                    Data = total,
                    ReturnMsg = "تم حساب إجمالي السلف بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetSummary)}");
                return Ok(new TResponse<decimal> { Success = false, ReturnMsg = ex.Message });
            }
        }
    }
}
