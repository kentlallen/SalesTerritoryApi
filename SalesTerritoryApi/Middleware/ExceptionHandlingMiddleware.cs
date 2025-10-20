using System.Net;
using System.Text.Json;
using FluentValidation;

namespace SalesTerritoryApi.Middleware
{
    // Global exception handler - catches all unhandled exceptions and returns consistent error responses
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue to the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            // Handle different exception types with appropriate HTTP status codes
            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        title = "Validation failed",
                        status = (int)HttpStatusCode.BadRequest,
                        detail = "One or more validation errors occurred.",
                        errors = validationException.Errors.ToDictionary(
                            e => e.PropertyName,
                            e => new[] { e.ErrorMessage }
                        )
                    };
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        title = "Invalid argument",
                        status = (int)HttpStatusCode.BadRequest,
                        detail = exception.Message
                    };
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new
                    {
                        title = "Resource not found",
                        status = (int)HttpStatusCode.NotFound,
                        detail = exception.Message
                    };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new
                    {
                        title = "An error occurred",
                        status = (int)HttpStatusCode.InternalServerError,
                        detail = "An unexpected error occurred while processing your request."
                    };
                    break;
            }

            // Serialize response with camelCase naming to match frontend expectations
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
