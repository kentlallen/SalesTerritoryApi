using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using System.Linq;

/// <summary>
/// This action filter will handle model validation for all controller actions using FluentValidation
/// This approach offers a more flexible, testable, and maintainable way to handle validation compared to DataAnnotations
/// </summary>
public class ValidationFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Validates the model for each action parameter using FluentValidation
    /// </summary>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check FluentValidation for each action parameter
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            var argument = context.ActionArguments[parameter.Name];
            if (argument == null) continue;

            // Get the validator for this parameter type
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
            
            if (validator != null)
            {
                // Validate the model using custom validation rules
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext);
                
                if (!validationResult.IsValid)
                {
                    // Build a ValidationProblemDetails object to return a 422 Unprocessable Entity response
                    var problemDetails = new ValidationProblemDetails
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "One or more validation errors occurred",
                    };

                    problemDetails.Errors = new Dictionary<string, string[]>(validationResult.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage }));

                    // Short-circuit the action execution and return the 422 Unprocessable Entity response
                    context.Result = new UnprocessableEntityObjectResult(problemDetails);
                    return;
                }
            }
        }

        // Continue to the next middleware in the pipeline or the controller action
        await next();
    }
}