using Application.Interface;
using AppModels.Common;
using AppModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class StoreController(IStoreInventoryServices services, ILogger<StoreController> logger) : ControllerBase
    {
        // ============================================================
        // 📋 جلب كل المواد المخزنة
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<StoreSummaryDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<StoreSummaryDto>>()
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع المواد المخزنة بنجاح.",
                    Data = result.ToList(),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<StoreSummaryDto>>()
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المواد المخزنة: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب مادة حسب رقم النوع
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<StoreSummaryDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByMaterialTypeIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<StoreSummaryDto> { Success = false, ReturnMsg = "لم يتم العثور على المادة المطلوبة." });

                return Ok(new TResponse<StoreSummaryDto> { Success = true, Data = result, ReturnMsg = "تم جلب المادة بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<StoreSummaryDto> { Success = false, ReturnMsg = "حدث خطأ أثناء جلب المادة: " + ex.Message });
            }
        }

        // ============================================================
        // ➕ إضافة مادة جديدة
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] StoreSummaryDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "تمت إضافة المادة بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = "حدث خطأ أثناء إضافة المادة: " + ex.Message });
            }
        }

        // ============================================================
        // ✏️ تعديل مادة
        // ============================================================
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] StoreSummaryDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "تم تعديل المادة بنجاح." : "فشل في تعديل المادة." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء تعديل المادة: " + ex.Message });
            }
        }

        // ============================================================
        // 🗑️ حذف مادة
        // ============================================================
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await services.DeleteAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "تم حذف المادة بنجاح." : "فشل في حذف المادة." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء حذف المادة: " + ex.Message });
            }
        }

        // ============================================================
        // 🔍 التحقق من وجود مادة
        // ============================================================
        [HttpGet("Exists/{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> Exists(Guid id)
        {
            try
            {
                var exists = await services.ExistsAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = true,
                    Data = exists,
                    ReturnMsg = exists ? "المادة موجودة في المخزون." : "المادة غير موجودة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Exists)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء التحقق من وجود المادة: " + ex.Message });
            }
        }
    }
}
