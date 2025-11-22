using Application.Interface.Employees;
using AppModels.Common;
using AppModels.Models.Employees;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactoryAPI.Controllers.Employees
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class EmployeeManagementController(IEmployeeManagementService service, ILogger<EmployeeManagementController> logger) : ControllerBase
    {

        #region ================= Employee CRUD =================
        // ============================================================
        // 📋 جلب جميع الموظفين مع التصفية Pagination
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<TResponse<IEnumerable<EmployeeDto>>>> GetAll([FromBody] PaginationEntity param)
        {
            if (param == null)
                return BadRequest("معايير البحث غير موجودة.");

            try
            {
                var employees = await service.GetAllEmployeesAsync(param);

                if (employees == null)
                    return NotFound("لم يتم العثور على موظفين.");

                return Ok(new TResponse<IEnumerable<EmployeeDto>> { Success = true, Data = employees.Data,TotalCount=employees.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAll)}");
                return Ok(new TResponse<IEnumerable<EmployeeDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult<TResponse<IEnumerable<EmployeeDto>>>> GetAll()
        {
            try
            {
                var data = await service.GetAllEmployeesAsync();
                return Ok(new TResponse<IEnumerable<EmployeeDto>> { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAll)}");
                return Ok(new TResponse<IEnumerable<EmployeeDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<EmployeeDto>>> Get(Guid id)
        {
            try
            {
                var result = await service.GetEmployeeByIdAsync(id);
                return Ok(new TResponse<EmployeeDto> { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Get)}");
                return Ok(new TResponse<EmployeeDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Create([FromBody] EmployeeDto dto)
        {
            try
            {
                var id = await service.AddEmployeeAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تم إضافة الموظف بنجاح" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Create)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] EmployeeDto dto)
        {
            try
            {
                var result = await service.UpdateEmployeeAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات الموظف" : "❌ لم يتم التعديل"
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
                var result = await service.DeleteEmployeeAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم حذف الموظف" : "❌ فشل الحذف"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        #endregion


        #region ================= Cash Advances =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddCashAdvance(EmployeeCashAdvanceDto dto)
        {
            try
            {
                var id = await service.AddCashAdvanceAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تمت إضافة السلفة" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddCashAdvance)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeleteCashAdvance(Guid id)
        {
            try
            {
                var result = await service.DeleteCashAdvanceAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف السلفة" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeleteCashAdvance)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost("{employeeId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<EmployeeCashAdvanceDto>>>> GetCashAdvances(Guid employeeId, PaginationEntity param)
        {
            try
            {
                var result = await service.GetCashAdvancesAsync(employeeId, param);
                return Ok(new TResponse<IEnumerable<EmployeeCashAdvanceDto>> { Success = true, Data = result.Data ,TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetCashAdvances)}");
                return Ok(new TResponse<IEnumerable<EmployeeCashAdvanceDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdateEmployeeCashAdvance([FromBody] EmployeeCashAdvanceDto dto)
        {
            try
            {
                var result = await service.UpdateEmployeeCashAdvanceAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات الموظف" : "❌ لم يتم التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdateEmployeeCashAdvance)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }
        #endregion


        #region ================= Personal Expenses =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddPersonalExpense(EmployeePersonalExpenseDto dto)
        {
            try
            {
                var id = await service.AddPersonalExpenseAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تمت إضافة المصروف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddPersonalExpense)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeletePersonalExpense(Guid id)
        {
            try
            {
                var result = await service.DeletePersonalExpenseAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف المصروف" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeletePersonalExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost("{employeeId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<EmployeePersonalExpenseDto>>>> GetPersonalExpenses(Guid employeeId, PaginationEntity param)
        {
            try
            {
                var result = await service.GetPersonalExpensesAsync(employeeId, param);
                return Ok(new TResponse<IEnumerable<EmployeePersonalExpenseDto>> { Success = true, Data = result.Data,TotalCount = result.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetPersonalExpenses)}");
                return Ok(new TResponse<IEnumerable<EmployeePersonalExpenseDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdatePersonalExpense([FromBody] EmployeePersonalExpenseDto dto)
        {
            try
            {
                var result = await service.UpdatePersonalExpenseAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات الموظف" : "❌ لم يتم التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdatePersonalExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }
        #endregion
    }
}
