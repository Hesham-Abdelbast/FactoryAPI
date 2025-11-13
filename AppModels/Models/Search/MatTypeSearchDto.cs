using Ejd.GRC.AppModels.Common;

namespace AppModels.Models.Search
{
    public sealed class MatTypeSearchDto : PaginationEntity
    {
        public string? MatTypeName { get; set; }
    }
}