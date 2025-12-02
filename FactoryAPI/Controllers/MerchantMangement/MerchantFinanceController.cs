using Application.Interface.MerchantMangement;
using AppModels.Common;
using AppModels.Models.MerchantMangement;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers.MerchantMangement
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class MerchantFinanceController(IMerchantFinanceServices services, ILogger<MerchantFinanceController> logger) : ControllerBase
    {

        // ============================================================
        // 📋 Get All Finance Records (No Pagination)
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<TResponse<List<MerchantFinanceDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = true,
                    Data = result.ToList(),
                    ReturnMsg = "تم جلب البيانات بنجاح"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        // ============================================================
        // 📋 Get All With Pagination
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<List<MerchantFinanceDto>>>> GetAllWithPagination(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);

                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب البيانات بنجاح.",
                    Data = result.Data.ToList(),
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllWithPagination)}");
                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء الجلب: " + ex.Message
                });
            }
        }

        // ============================================================
        // 📋 Get By Merchant With Pagination
        // ============================================================
        [HttpPost("{id:guid}")]
        public async Task<ActionResult<TResponse<List<MerchantFinanceDto>>>> GetAllByMerchantIdWithPagination(Guid id, PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllByMerchantIdAsync(id, param);

                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب البيانات بنجاح",
                    Data = result.Data.ToList(),
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllByMerchantIdWithPagination)}");
                return Ok(new TResponse<List<MerchantFinanceDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        // ============================================================
        // 🔍 Get By ID
        // ============================================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<MerchantFinanceDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);

                if (result == null)
                {
                    return Ok(new TResponse<MerchantFinanceDto>
                    {
                        Success = false,
                        ReturnMsg = "السجل غير موجود."
                    });
                }

                return Ok(new TResponse<MerchantFinanceDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<MerchantFinanceDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ➕ Add Finance Record
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] MerchantFinanceDto dto)
        {
            try
            {
                var id = await services.CreateAsync(dto);

                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تم إضافة التمويل بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ✏️ Update Finance
        // ============================================================
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] MerchantFinanceDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);

                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل التمويل بنجاح" : "فشل التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ============================================================
        // ❌ Delete Finance
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
                    ReturnMsg = result ? "تم حذف السجل بنجاح" : "فشل الحذف"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }
    }
}