using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using System.Net;

namespace DebtsCompass
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            string message;

            switch (ex)
            {
                case UserNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case InvalidCredentialsException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case ForbiddenRequestException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case EmailAlreadyExistsException:
                    statusCode = HttpStatusCode.Conflict;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case InvalidEmailException:
                case InvalidPasswordException:
                case PasswordMismatchException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    logger.LogError("An exception occurred while processing the request.");
                    break;
            }

            context.Response.ContentType = "application/json";
            Response<object> response = new(message, null, statusCode);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
