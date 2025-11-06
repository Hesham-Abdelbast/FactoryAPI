using Application.Interface;
using AppModels.Common;
using AppModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class TransactionsController(ITransactionServices services, ILogger<TransactionsController> logger) : ControllerBase
    {
        // ============================================================
        // 📋 جلب كل المعاملات
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<TransactionDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<TransactionDto>>()
                {
                    Success = true,
                    Data = result.ToList(),
                    ReturnMsg = "تم جلب جميع المعاملات بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<TransactionDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المعاملات: " + ex.Message
                });
            }
        }

        // ============================================================
        // 📋 جلب كل المعاملات
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<TransactionDto>>>> GetAllByMerchantId(Guid merchantId)
        {
            try
            {
                var result = await services.GetAllByMerchantIdAsync(merchantId);
                return Ok(new TResponse<List<TransactionDto>>()
                {
                    Success = true,
                    Data = result.ToList(),
                    ReturnMsg = "تم جلب جميع المعاملات بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllByMerchantId)}");
                return Ok(new TResponse<List<TransactionDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المعاملات: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب معاملة حسب رقمها
        // ============================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<TransactionDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<TransactionDto> { Success = false, ReturnMsg = "لم يتم العثور على المعاملة المطلوبة." });

                return Ok(new TResponse<TransactionDto> { Success = true, Data = result, ReturnMsg = "تم جلب المعاملة بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<TransactionDto> { Success = false, ReturnMsg = "حدث خطأ أثناء جلب المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // ➕ إضافة معاملة جديدة
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] TransactionDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تم إضافة المعاملة بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = "حدث خطأ أثناء إضافة المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // ✏️ تحديث معاملة موجودة
        // ============================================================
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] TransactionDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل المعاملة بنجاح." : "فشل في تعديل المعاملة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء تعديل المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // 🗑️ حذف معاملة
        // ============================================================
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await services.DeleteAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم حذف المعاملة بنجاح." : "فشل في حذف المعاملة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء حذف المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // 🔍 التحقق من وجود معاملة برقم معين
        // ============================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Exists(Guid id)
        {
            try
            {
                var exists = await services.ExistsAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = true,
                    Data = exists,
                    ReturnMsg = exists ? "المعاملة موجودة." : "المعاملة غير موجودة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Exists)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء التحقق من وجود المعاملة: " + ex.Message });
            }
        }
    }
}
