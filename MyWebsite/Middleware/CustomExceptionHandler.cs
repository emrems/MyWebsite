using MyWebsite.Dtos.Response;
using MyWebsite.Exceptions;

namespace MyWebsite.Middleware
{
    public static class CustomExceptionHandler
    {
        public static Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode;
            string type;

            switch (exception)
            {
                case UnAuthorizedException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    type = "Unauthorized";
                    break;
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    type = "Not Found";
                    break;
                case BadRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    type = "Bad Request";
                    break;
                case ForbiddenException:
                    statusCode = StatusCodes.Status403Forbidden;
                    type = "Forbidden";
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    type = "Internal Server Error";
                    break;
            }
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response = new BaseResponse<object>
            {
                IsSuccess = false,
                Message = exception.Message,
                Data = null,
                ErrroCode = statusCode.ToString()
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
