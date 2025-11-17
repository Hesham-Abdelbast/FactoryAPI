using Application.Interface.Equipments;
using AppModels.Common;
using AppModels.Models.Equipments;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactoryAPI.Controllers.Equipments
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class EquipmentManagementController : ControllerBase
    {
        private readonly IEquipmentManagementService service;
        private readonly ILogger<EquipmentManagementController> logger;

        public EquipmentManagementController(IEquipmentManagementService service, ILogger<EquipmentManagementController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        #region ================= Equipment CRUD =================
        // ============================================================
        // 📋 جلب جميع المعدات مع التصفية Pagination
        // ============================================================
        [HttpPost()]
        public async Task<ActionResult<TResponse<IEnumerable<EquipmentDto>>>> GetAll([FromBody] PaginationEntity param)
        {
            if (param == null)
                return BadRequest(new TResponse<IEnumerable<EquipmentDto>>
                {
                    Success = false,
                    ReturnMsg = "معايير البحث غير موجودة."
                });

            try
            {
                var equipments = await service.GetAllEquipmentsAsync(param);

                if (equipments == null || !equipments.Any())
                    return Ok(new TResponse<IEnumerable<EquipmentDto>>
                    {
                        Success = true,
                        Data = Enumerable.Empty<EquipmentDto>(),
                        ReturnMsg = "لم يتم العثور على معدات."
                    });

                return Ok(new TResponse<IEnumerable<EquipmentDto>>
                {
                    Success = true,
                    Data = equipments
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAll)}");
                return Ok(new TResponse<IEnumerable<EquipmentDto>>
                {
                    Success = false,
                    ReturnMsg = $"حدث خطأ أثناء جلب المعدات: {ex.Message}"
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult<TResponse<IEnumerable<EquipmentDto>>>> GetAll()
        {
            try
            {
                var data = await service.GetAllEquipmentsAsync();
                return Ok(new TResponse<IEnumerable<EquipmentDto>> { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetAll)}");
                return Ok(new TResponse<IEnumerable<EquipmentDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TResponse<EquipmentDto>>> Get(Guid id)
        {
            try
            {
                var data = await service.GetEquipmentByIdAsync(id);
                return Ok(new TResponse<EquipmentDto> { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Get)}");
                return Ok(new TResponse<EquipmentDto> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> Create([FromBody] EquipmentDto dto)
        {
            try
            {
                var id = await service.AddEquipmentAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تم إضافة المعدة بنجاح" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Create)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] EquipmentDto dto)
        {
            try
            {
                var result = await service.UpdateEquipmentAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات المعدة" : "❌ لم يتم التعديل"
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
                var result = await service.DeleteEquipmentAsync(id);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم حذف المعدة" : "❌ فشل الحذف"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        #endregion

        #region ================= Equipment Expenses =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddExpense([FromBody] EquipmentExpenseDto dto)
        {
            try
            {
                var id = await service.AddEquipmentExpenseAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تمت إضافة المصروف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddExpense)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeleteExpense(Guid id)
        {
            try
            {
                var result = await service.DeleteEquipmentExpenseAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف المصروف" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeleteExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost("{equipmentId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<EquipmentExpenseDto>>>> GetExpenses(Guid equipmentId, PaginationEntity param)
        {
            try
            {
                var list = await service.GetEquipmentExpensesAsync(equipmentId,param);
                return Ok(new TResponse<IEnumerable<EquipmentExpenseDto>> { Success = true, Data = list });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetExpenses)}");
                return Ok(new TResponse<IEnumerable<EquipmentExpenseDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }
        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdateEquipmentExpense([FromBody] EquipmentExpenseDto dto)
        {
            try
            {
                var result = await service.UpdateEquipmentExpenseAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات المعدة" : "❌ لم يتم التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdateEquipmentExpense)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }


        #endregion

        #region ================= Equipment Incomes =================

        [HttpPost]
        public async Task<ActionResult<TResponse<Guid>>> AddIncome([FromBody] EquipmentIncomeDto dto)
        {
            try
            {
                var id = await service.AddEquipmentIncomeAsync(dto);
                return Ok(new TResponse<Guid> { Success = true, Data = id, ReturnMsg = "✔ تم إضافة الإيراد" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddIncome)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TResponse<bool>>> DeleteIncome(Guid id)
        {
            try
            {
                var result = await service.DeleteEquipmentIncomeAsync(id);
                return Ok(new TResponse<bool> { Success = result, ReturnMsg = result ? "✔ تم حذف الإيراد" : "❌ فشل الحذف" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(DeleteIncome)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPost("{equipmentId:guid}")]
        public async Task<ActionResult<TResponse<IEnumerable<EquipmentIncomeDto>>>> GetIncomes(Guid equipmentId, PaginationEntity param)
        {
            try
            {
                var list = await service.GetEquipmentIncomesAsync(equipmentId, param);
                return Ok(new TResponse<IEnumerable<EquipmentIncomeDto>> { Success = true, Data = list });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(GetIncomes)}");
                return Ok(new TResponse<IEnumerable<EquipmentIncomeDto>> { Success = false, ReturnMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<TResponse<bool>>> UpdateEquipmentIncome([FromBody] EquipmentIncomeDto dto)
        {
            try
            {
                var result = await service.UpdateEquipmentIncomeAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "✔ تم تعديل بيانات المعدة" : "❌ لم يتم التعديل"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UpdateEquipmentIncome)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = ex.Message });
            }
        }

        #endregion
    }
}