using Application.Interface.Drivers;
using AppModels.Common;
using AppModels.Models.Drivers;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers.Drivers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DriverManagementController(IDriverManagementService service, ILogger<DriverManagementController> logger) : ControllerBase
    {

        #region ================= Driver CRUD =================

        [HttpPost]
        public async Task<ActionResult<TResponse<IEnumerable<DriverDto>>>> GetAll([FromBody] PaginationEntity param)
        {
            try
            {
                var drivers = await service.GetAllDriversAsync(param);

                return Ok(new TResponse<IEnumerable<DriverDto>>
                {
                    Success = true,
                    Data = drivers.Data,
                    TotalCount = drivers.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAll)}");
                return Ok(new TResponse<IEnumerable<DriverDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<DriverDto>>> Get(Guid id)
        {
            try
            {
                var result = await service.GetDriverByIdAsync(id);
                return Ok(new TResponse<DriverDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Get)}");
                return Ok(new TResponse<DriverDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Create([FromBody] CreateDriverDto dto)
        {
            try
            {
                var id = await service.AddDriverAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تم إضافة السائق بنجاح" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Create)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] CreateDriverDto dto)
        {
            try
            {
                var result = await service.UpdateDriverAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات السائق" : "❌ لم يتم التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> Delete(Guid id)
        {
            try
            {
                var result = await service.DeleteDriverAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم حذف السائق" : "❌ فشل الحذف"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        #endregion


        #region ================= Travel =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddTravel(CreateTravelDto dto)
        {
            try
            {
                var id = await service.AddTravelAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تمت إضافة الرحلة" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddTravel)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdateTravel(CreateTravelDto dto)
        {
            try
            {
                var result = await service.UpdateTravelAsync(dto);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم تعديل الرحلة" : "❌ فشل التعديل" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdateTravel)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeleteTravel(Guid id)
        {
            try
            {
                var result = await service.DeleteTravelAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف الرحلة" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeleteTravel)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TResponse<IEnumerable<TravelDto>>>> GetAllTravels([FromBody] PaginationEntity param)
        {
            try
            {
                var result = await service.GetAllTravelsAsync(param);
                return Ok(new TResponse<IEnumerable<TravelDto>> { Success = true, Data = result.Data, TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAllTravels)}");
                return Ok(new TResponse<IEnumerable<TravelDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost("{driverId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<TravelDto>>>> GetAllTravelsByDriverId(Guid driverId, [FromBody] PaginationEntity param)
        {
            try
            {
                var result = await service.GetAllTravelsByDriverIdAsync(driverId, param);
                return Ok(new TResponse<IEnumerable<TravelDto>> { Success = true, Data = result.Data, TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAllTravelsByDriverId)}");
                return Ok(new TResponse<IEnumerable<TravelDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }
        #endregion


        #region ================= Driver Expenses =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddExpense(CreateDriverExpenseDto dto)
        {
            try
            {
                var id = await service.AddDriverExpenseAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تمت إضافة المصروف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddExpense)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdateExpense(CreateDriverExpenseDto dto)
        {
            try
            {
                var result = await service.UpdateDriverExpenseAsync(dto);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم تعديل المصروف" : "❌ لم يتم التعديل" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdateExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeleteExpense(Guid id)
        {
            try
            {
                var result = await service.DeleteDriverExpenseAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف المصروف" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeleteExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TResponse<IEnumerable<DriverExpenseDto>>>> GetAllExpenses([FromBody] PaginationEntity param)
        {
            try
            {
                var result = await service.GetAllDriverExpensesAsync(param);
                return Ok(new TResponse<IEnumerable<DriverExpenseDto>> { Success = true, Data = result.Data, TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAllExpenses)}");
                return Ok(new TResponse<IEnumerable<DriverExpenseDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }
        [HttpPost("{driverId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<DriverExpenseDto>>>> GetAllExpensesByDriverId(Guid driverId,[FromBody] PaginationEntity param)
        {
            try
            {
                var result = await service.GetAllDriverExpensesByDriverIdAsync(driverId,param);
                return Ok(new TResponse<IEnumerable<DriverExpenseDto>> { Success = true, Data = result.Data, TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAllExpensesByDriverId)}");
                return Ok(new TResponse<IEnumerable<DriverExpenseDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        #endregion
    }
}
