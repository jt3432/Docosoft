namespace Docosoft.API.Models.Response
{
    public class ErrorResponse : ApiResponseBase
    {
        public ErrorResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }
}
