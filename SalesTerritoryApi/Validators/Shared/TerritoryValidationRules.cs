using System.Text.RegularExpressions;
using FluentValidation;

namespace SalesTerritoryApi.Validators.Shared
{
    /// <summary>
    /// Shared validation rules for territory DTOs to eliminate code duplication
    /// </summary>
    public static class TerritoryValidationRules
    {
        /// <summary>
        /// Applies common territory validation rules to any territory DTO
        /// </summary>
        public static IRuleBuilder<T, string> ApplyNameValidation<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must be less than 100 characters");
        }

        /// <summary>
        /// Applies zip code validation rules
        /// </summary>
        public static IRuleBuilder<T, List<string>> ApplyZipCodeValidation<T>(this IRuleBuilder<T, List<string>> rule)
        {
            return rule
                .NotEmpty()
                .WithMessage("At least one zip code is required");
        }

        /// <summary>
        /// Applies comprehensive zip code validation to the entire collection
        /// </summary>
        public static IRuleBuilder<T, List<string>> ApplyZipCodeCollectionValidation<T>(this IRuleBuilder<T, List<string>> rule)
        {
            return rule
                .NotEmpty()
                .WithMessage("At least one zip code is required")
                .Must(zipCodes => zipCodes.All(zip => System.Text.RegularExpressions.Regex.IsMatch(zip, @"^\d{5}$")))
                .WithMessage("All zip codes must be valid 5-digit numbers");
        }

        /// <summary>
        /// Applies demographics validation rules
        /// </summary>
        public static IRuleBuilder<T, Dictionary<string, object>> ApplyDemographicsValidation<T>(this IRuleBuilder<T, Dictionary<string, object>> rule)
        {
            return rule.Custom((demographics, context) =>
            {
                // Allow empty demographics dictionary
                if (demographics.Count == 0)
                {
                    return;
                }

                foreach (var demographic in demographics)
                {
                    if (string.IsNullOrEmpty(demographic.Key) || string.IsNullOrEmpty(demographic.Value?.ToString()))
                    {
                        context.AddFailure("demographics", $"\"{demographic.Key}:{demographic.Value}\" must contain both a name and value {{<name>:<value>}}");
                    }
                }
            });
        }
    }
}
