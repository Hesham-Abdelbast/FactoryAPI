using Ejd.GRC.AppModels.Common;

namespace AppModels.Common
{
    public sealed class TResponse<T>:PaginationEntity 
    {
        public bool Success { get; set; }
        public string? ReturnCode { get; set; }
        public string? ReturnMsg { get; set; }
        public T? Data { get; set; }
    }
}
