
namespace Ejd.GRC.AppModels.Common
{
    public class PaginationEntity
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public int? TotalCount { get; set; }//required from the second call
        public string? Search { get; set; }
    }
}
