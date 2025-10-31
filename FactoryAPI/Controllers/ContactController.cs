using Application.Interface;
using AppModels.Common;
using AppModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ContactController(IContactServices services, ILogger<ContactController> logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<ContactDto>>> GetContactAsync()
        {
            try
            {
                var result = await services.GetContactAsync();
                return Ok(new TResponse<ContactDto>()
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetContactAsync)}");
                return Ok(new TResponse<ContactDto>
                {
                    Success = false,
                    ReturnMsg = ex.Message
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] ContactDto dto)
        {
            try
            {
                var result = await services.UpdateContactAsync(dto);
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
    }
}
