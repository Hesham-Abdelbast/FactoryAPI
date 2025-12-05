using Application.Interface;
using AppModels.Common;
using AppModels.Models.Search;
using AppModels.Models.Transaction;
using Ejd.GRC.AppModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class TransactionsController(ITransactionServices services, ILogger<TransactionsController> logger) : ControllerBase
    {

        // ============================================================
        // 📋 البحث في كل المعاملات
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TResponse<List<TransactionDto>>>> Search(TxnSearchDto searchDto)
        {
            try
            {
                if (searchDto == null)
                {
                    return Ok(new TResponse<List<TransactionDto>>
                    {
                        Success = false,
                        ReturnMsg = "معايير البحث لا يمكن أن تكون فارغة."
                    });
                }

                var result = await services.SearchAsync(searchDto);

                return Ok(new TResponse<List<TransactionDto>>
                {
                    Success = true,
                    ReturnMsg = result.Data.Any()
                        ? "تم جلب نتائج البحث بنجاح."
                        : "لم يتم العثور على نتائج مطابقة لمعايير البحث.",
                    Data = result.Data.ToList(),
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Search)}");

                return Ok(new TResponse<List<TransactionDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء تنفيذ عملية البحث: " + ex.Message
                });
            }
        }


        // ============================================================
        // 📋 جلب كل المعاملات
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<List<TransactionDto>>>> GetAll(PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllAsync(param);
                return Ok(new TResponse<List<TransactionDto>>()
                {
                    Success = true,
                    Data = result.Data.ToList(),
                    ReturnMsg = "تم جلب جميع المعاملات بنجاح.",
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAll)}");
                return Ok(new TResponse<List<TransactionDto>>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المعاملات: " + ex.Message
                });
            }
        }

        // ============================================================
        // 📋 جلب كل المعاملات
        // ============================================================
        [HttpPost("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<AllTransByMerchantDto>>> GetAllByMerchantId(Guid id,PaginationEntity param)
        {
            try
            {
                var result = await services.GetAllByMerchantIdAsync(id, param);
                return Ok(new TResponse<AllTransByMerchantDto>()
                {
                    Success = true,
                    Data = result.Data,
                    ReturnMsg = "تم جلب جميع المعاملات بنجاح.",
                    TotalCount = result.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetAllByMerchantId)}");
                return Ok(new TResponse<AllTransByMerchantDto>
                {
                    Success = false,
                    ReturnMsg = "حدث خطأ أثناء جلب المعاملات: " + ex.Message
                });
            }
        }

        // ============================================================
        // 🔎 جلب معاملة حسب رقمها
        // ============================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<TransactionDto>>> GetById(Guid id)
        {
            try
            {
                var result = await services.GetByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<TransactionDto> { Success = false, ReturnMsg = "لم يتم العثور على المعاملة المطلوبة." });

                return Ok(new TResponse<TransactionDto> { Success = true, Data = result, ReturnMsg = "تم جلب المعاملة بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<TransactionDto> { Success = false, ReturnMsg = "حدث خطأ أثناء جلب المعاملة: " + ex.Message });
            }
        }
        // ============================================================
        // 🔎 جلب الفاتوره حسب رقمها
        // ============================================================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<InvoiceDto?>>> GetInvoiceByIdAsync(Guid id)
        {
            try
            {
                var result = await services.GetInvoiceByIdAsync(id);
                if (result == null)
                    return Ok(new TResponse<InvoiceDto> { Success = false, ReturnMsg = "لم يتم العثور على الفاتوره المطلوبة." });

                return Ok(new TResponse<InvoiceDto> { Success = true, Data = result, ReturnMsg = "تم جلب الفاتوره بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetById)}");
                return Ok(new TResponse<InvoiceDto> { Success = false, ReturnMsg = "حدث خطأ أثناء جلب الفاتوره: " + ex.Message });
            }
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<InvoiceLstDto?>>> GetInvoiceByIdsAsync(List<Guid> ids)
        {
            try
            {
                var result = await services.GetInvoiceByIdsAsync(ids);
                if (result == null)
                    return Ok(new TResponse<InvoiceLstDto> { Success = false, ReturnMsg = "لم يتم العثور على الفاتوره المطلوبة." });

                return Ok(new TResponse<InvoiceLstDto> { Success = true, Data = result, ReturnMsg = "تم جلب الفاتوره بنجاح." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(GetInvoiceByIdsAsync)}");
                return Ok(new TResponse<InvoiceLstDto> { Success = false, ReturnMsg = "حدث خطأ أثناء جلب الفاتوره: " + ex.Message });
            }
        }
        // ============================================================
        // ➕ إضافة معاملة جديدة
        // ============================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<Guid>>> Add([FromBody] CreateTransactionDto dto)
        {
            try
            {
                var id = await services.AddAsync(dto);
                return Ok(new TResponse<Guid>
                {
                    Success = true,
                    Data = id,
                    ReturnMsg = "تم إضافة المعاملة بنجاح."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Add)}");
                return Ok(new TResponse<Guid> { Success = false, ReturnMsg = "حدث خطأ أثناء إضافة المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // ✏️ تحديث معاملة موجودة
        // ============================================================
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TResponse<bool>>> Update([FromBody] CreateTransactionDto dto)
        {
            try
            {
                var result = await services.UpdateAsync(dto);
                return Ok(new TResponse<bool>
                {
                    Success = result,
                    ReturnMsg = result ? "تم تعديل المعاملة بنجاح." : "فشل في تعديل المعاملة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Update)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء تعديل المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // 🗑️ حذف معاملة
        // ============================================================
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
                    ReturnMsg = result ? "تم حذف المعاملة بنجاح." : "فشل في حذف المعاملة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Delete)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء حذف المعاملة: " + ex.Message });
            }
        }

        // ============================================================
        // 🔍 التحقق من وجود معاملة برقم معين
        // ============================================================
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
                    Data = exists,
                    ReturnMsg = exists ? "المعاملة موجودة." : "المعاملة غير موجودة."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{GetType().Name}.{nameof(Exists)}");
                return Ok(new TResponse<bool> { Success = false, ReturnMsg = "حدث خطأ أثناء التحقق من وجود المعاملة: " + ex.Message });
            }
        }
    }
}
