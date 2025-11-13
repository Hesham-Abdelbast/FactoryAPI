
using Ejd.GRC.AppModels.Common;

namespace AppModels.Models.Search
{
    public sealed class TxnSearchDto : PaginationEntity
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? MerchantName { get; set; }
        public string? MaterialTypeName { get; set; }
        public string? WarehouseName { get; set; }

    }
}
