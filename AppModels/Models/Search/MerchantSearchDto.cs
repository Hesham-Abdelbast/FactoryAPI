using Ejd.GRC.AppModels.Common;

namespace AppModels.Models.Search
{
    public sealed class MerchantSearchDto : PaginationEntity
    {
        public string? MerchantName { get; set; }
    }
}
