namespace Docosoft.API.Models.Response
{
    public class SuccessResponse<T> : ApiResponseBase
    {
        public T? Data { get; set; }

        public SuccessResponse(T? data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }
    }
}
