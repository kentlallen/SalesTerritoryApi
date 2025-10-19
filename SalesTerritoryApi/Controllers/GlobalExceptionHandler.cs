using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SalesTerritoryApi.Controllers
{
    public class GlobalExceptionHandler(IWebHostEnvironment _env, ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            // Log the exception
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            string detail = "We are sorry, something went wrong on our end. Please try again later.";
            
            // Only show details if development environment to avoid sharing sensitive details with user
            if (_env.IsDevelopment())
            {
                detail = exception.Message;
            }

            // Create a custom ProblemDetails response
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = detail
            }, cancellationToken);

            return true; // Indicates the exception is handled
        }
    }
}