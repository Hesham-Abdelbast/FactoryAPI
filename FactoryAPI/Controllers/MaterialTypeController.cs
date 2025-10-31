using Application.Interface;
using AppModels.Common;
using AppModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class MaterialTypeController(IMaterialTypeServices services, ILogger<MaterialTypeController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<MaterialTypeDto>>>> GetAll()
        {
            try
            {
                var result = await services.GetAllAsync();
                return Ok(new TResponse<List<MaterialTypeDto>>()
                {
                    Success = true,
                    Data = result.ToList()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<MaterialTypeDto>>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<MaterialTypeDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<MaterialTypeDto> { Success = false, ReturnMsg = "Material Type not found." });

                return Ok(new TResponse<MaterialTypeDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<MaterialTypeDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] MaterialTypeDto dto)
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
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] MaterialTypeDto dto)
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