using Docosoft.API.Models.Response;

namespace Docosoft.API.Factories
{
    public static class ApiResponseFactory
    {
        public static ApiResponseBase CreateSuccess<T>(T? data, string message = "")
        {
            return new SuccessResponse<T>(data, message);
        }

        public static ApiResponseBase CreateError(string message)
        {
            return new ErrorResponse(message);
        }
    }
}
