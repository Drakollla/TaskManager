using Domain.ErrorModel;
using Domain.Exceptions;
using System.Net;

namespace TaskManagerAPI.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error from the custom middleware.";

            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = string.Join("; ", validationEx.Errors.Select(x => x.ErrorMessage));
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case BadRequestException badRequestEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = badRequestEx.Message;
                    break;

                default:
                    message = exception.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = statusCode,
                Message = message
            }.ToString());
        }
    }
}
