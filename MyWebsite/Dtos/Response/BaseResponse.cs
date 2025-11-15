namespace MyWebsite.Dtos.Response
{
    public class BaseResponse<T> where T : class
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ErrroCode { get; set; }
    }
}
