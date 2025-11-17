namespace AppModels.Common
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; } = [];
    }
}
