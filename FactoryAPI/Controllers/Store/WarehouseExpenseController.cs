using Application.Interface.Store;
using AppModels.Common;
using AppModels.Models.Store;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers.Store
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class WarehouseExpenseController(IWarehouseExpenseServices services, ILogger<WarehouseExpenseController> logger)
        : ControllerBase
    {
        // ============================================================
        // 📋 Get all expenses (Paginated)
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseExpenseDto>>>> GetAllWithPagination(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<WarehouseExpenseDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع مصاريف المخازن بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllWithPagination)}");
                return Ok(new TResponse<List<WarehouseExpenseDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب مصاريف المخازن: " + ex.Message
                });
            }
        }

        // ============================================================
        // 📋 Get all expenses
        // ============================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<WarehouseExpenseDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<WarehouseExpenseDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع مصاريف المخازن بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<WarehouseExpenseDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب مصاريف المخازن: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 Get expense by id
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<WarehouseExpenseDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);

                if (result == null)
                    return Ok(new TResponse<WarehouseExpenseDto>
                    {
                        Success = false,
                        ReturnMsg = "لم يتم العثور على المصروف المطلوب."
                    });

                return Ok(new TResponse<WarehouseExpenseDto>
                {
                    Success = true,
                    Data = result,
                    ReturnMsg = "تم جلب المصروف بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<WarehouseExpenseDto>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المصروف: " + ex.Message
                });
            }
        }

        // ============================================================
        // ➕ Add new expense
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] WarehouseExpenseDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تمت إضافة المصروف بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء إضافة المصروف: " + ex.Message
                });
            }
        }

        // ============================================================
        // ✏️ Update expense
        // ============================================================
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] WarehouseExpenseDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل المصروف بنجاح." : "فشل في تعديل المصروف."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء تعديل المصروف: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🗑️ Delete expense (Soft Delete)
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
                    ReturnMsg = result ? "تم حذف المصروف بنجاح." : "فشل في حذف المصروف."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء حذف المصروف: " + ex.Message
                });
            }
        }
    }
}
