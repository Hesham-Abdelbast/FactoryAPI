using Application.Interface.Store;
using AppModels.Common;
using AppModels.Models.Store;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers.Store
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class WarehouseController(IWarehouseServices services, ILogger<WarehouseController> logger) : ControllerBase
    {
        // ============================================================
        // 📋 جلب كل المخازن
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseDto>>>> GetAllWithPagination(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<WarehouseDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع المخازن بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<WarehouseDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المخازن: " + ex.Message
                });
            }
        }
        // ============================================================
        // 📋 جلب كل المخازن
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<WarehouseDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع المخازن بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<WarehouseDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المخازن: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب مخزن حسب رقم المعرف
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<WarehouseDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);

                if (result == null)
                    return Ok(new TResponse<WarehouseDto>
                    {
                        Success = false,
                        ReturnMsg = "لم يتم العثور على المخزن المطلوب."
                    });

                return Ok(new TResponse<WarehouseDto>
                {
                    Success = true,
                    Data = result,
                    ReturnMsg = "تم جلب المخزن بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<WarehouseDto>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المخزن: " + ex.Message
                });
            }
        }

        // ============================================================
        // ➕ إضافة مخزن جديد
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] WarehouseDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تمت إضافة المخزن بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء إضافة المخزن: " + ex.Message
                });
            }
        }

        // ============================================================
        // ✏️ تعديل بيانات مخزن
        // ============================================================
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] WarehouseDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل بيانات المخزن بنجاح." : "فشل في تعديل بيانات المخزن."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء تعديل بيانات المخزن: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🗑️ حذف مخزن (Soft Delete)
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
                    ReturnMsg = result ? "تم حذف المخزن بنجاح." : "فشل في حذف المخزن."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء حذف المخزن: " + ex.Message
                });
            }
        }
    }
}
