namespace AppModels.Common
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public T Data { get; set; }
    }
}
