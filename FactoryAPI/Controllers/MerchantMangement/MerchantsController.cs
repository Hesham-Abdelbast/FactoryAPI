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
    public class MerchantsController(IMerchantServices services, ILogger<MerchantsController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<MerchantDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<MerchantDto>>()
                {
                    Success = true,
                    Data = result.ToList()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<MerchantDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<MerchantDto>>>> GetAll(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<MerchantDto>>()
                {
                    Success = true,
                    Data = result.Data.ToList(),
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<MerchantDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<MerchantDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<MerchantDto> { Success = false, ReturnMsg = "Material Type not found." });

                return Ok(new TResponse<MerchantDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<MerchantDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] MerchantDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "Material Type added successfully."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] MerchantDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "Material Type updated successfully." : "Update failed."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

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
                    ReturnMsg = result ? "Material Type deleted successfully." : "Delete failed."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

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