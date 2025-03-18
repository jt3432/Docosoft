namespace Docosoft.API.Models.Response
{
    public abstract class ApiResponseBase
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = String.Empty;
    }

    
}
