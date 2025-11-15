using Application.Interface;
using AppModels.Common;
using AppModels.Models;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class FinancingController(IFinancingService services, ILogger<FinancingController> logger) : ControllerBase
    {
        // ============================================================
        // 📋 جلب كل التمويلات مع التصفية (Pagination)
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<FinancingDto>>>> GetAllWithPagination([FromBody] PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<FinancingDto>>
                {
                    Success = true,
                    ReturnMsg = "تم جلب جميع التمويلات بنجاح.",
                    Data = result.ToList(),
                    TotalCount = result.Count()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllWithPagination)}");
                return Ok(new TResponse<List<FinancingDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب التمويلات: " + ex.Message
                });
            }
        }

        // ================================================================
        // 📌 GET ALL
        // ================================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<FinancingDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<FinancingDto>>
                {
                    Success = true,
                    Data = result.ToList()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<FinancingDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ================================================================
        // 📌 GET BY ID
        // ================================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<FinancingDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);

                if (result == null)
                    return Ok(new TResponse<FinancingDto> { Success = false, ReturnMsg = "Financing record not found." });

                return Ok(new TResponse<FinancingDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<FinancingDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ================================================================
        // ➕ ADD
        // ================================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] FinancingCreateDto dto)
        {
            try
            {
                var id = await services.CreateAsync(dto);

                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "Financing record added successfully."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ================================================================
        // 🔁 UPDATE
        // ================================================================
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] FinancingDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);

                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "Financing record updated successfully." : "Update failed."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ================================================================
        // 🗑 DELETE
        // ================================================================
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
                    ReturnMsg = result ? "Financing record deleted successfully." : "Delete failed."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        // ================================================================
        // ❓ EXISTS CHECK
        // ================================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Exists(Guid id)
        {
            try
            {
                var exists = await services.GetByIdAsync(id) != null;

                return Ok(new TResponse<bool>
                {
                    Success = true,
                    Data = exists
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Exists)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }
    }
}
