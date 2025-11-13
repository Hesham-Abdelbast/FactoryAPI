using Application.Interface;
using AppModels.Common;
using AppModels.Models.Warehouse;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class WarehouseInventoryController(IWarehouseInventoryServices services, ILogger<WarehouseInventoryController> logger) : ControllerBase
    {
        // ============================================================
        // 📋 جلب كل عناصر المخزون (مع ترقيم الصفحات)
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseInventoryDto>>>> GetAllWithPagination(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع عناصر المخزون بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllWithPagination)}");
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب عناصر المخزون: " + ex.Message
                });
            }
        }

        // ============================================================
        // 📋 جلب كل عناصر المخزون (بدون ترقيم)
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseInventoryDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع عناصر المخزون بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب عناصر المخزون: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب عنصر حسب رقم المعرف
        // ============================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<WarehouseInventoryDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);

                if (result == null)
                    return Ok(new TResponse<WarehouseInventoryDto>
                    {
                        Success = false,
                        ReturnMsg = "⚠ لم يتم العثور على العنصر المطلوب."
                    });

                return Ok(new TResponse<WarehouseInventoryDto>
                {
                    Success = true,
                    Data = result,
                    ReturnMsg = "✅ تم جلب العنصر بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<WarehouseInventoryDto>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب العنصر: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب العناصر حسب رقم المخزن
        // ============================================================
        [HttpGet("{warehouseId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseInventoryDto>>>> GetByWarehouseId(Guid warehouseId)
        {
            try
            {
                var result = await services.GetByWarehouseIdAsync(warehouseId);
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع عناصر المخزون الخاصة بالمخزن بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetByWarehouseId)}");
                return Ok(new TResponse<List<WarehouseInventoryDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب عناصر المخزون للمخزن: " + ex.Message
                });
            }
        }

        // ============================================================
        // ➕ إضافة عنصر جديد للمخزون
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] WarehouseInventoryDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "✅ تمت إضافة العنصر إلى المخزون بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء إضافة العنصر إلى المخزون: " + ex.Message
                });
            }
        }

        // ============================================================
        // ✏️ تعديل بيانات عنصر المخزون
        // ============================================================
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] WarehouseInventoryDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✅ تم تعديل بيانات العنصر بنجاح." : "⚠ فشل في تعديل بيانات العنصر."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء تعديل بيانات العنصر: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🗑️ حذف عنصر من المخزون (Soft Delete)
        // ============================================================
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await services.DeleteAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "🗑️ تم حذف العنصر بنجاح." : "⚠ فشل في حذف العنصر."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء حذف العنصر: " + ex.Message
                });
            }
        }
    }
}
