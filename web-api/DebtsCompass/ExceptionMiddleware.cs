using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using Microsoft.IdentityModel.Tokens;
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
                case EmailNotConfirmedException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case EmailAlreadyExistsException:
                case UsernameAlreadyExistsException:
                    statusCode = HttpStatusCode.Conflict;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case InvalidEmailException:
                case InvalidPasswordException:
                case PasswordMismatchException:
                case SecurityTokenException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case EmailAlreadyConfirmedException:
                    statusCode = HttpStatusCode.Gone;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case EntityNotFoundException:
                    statusCode = HttpStatusCode.NoContent;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    logger.LogError(message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    logger.LogError(ex.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            Response<object> response = new(message, null, statusCode);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
