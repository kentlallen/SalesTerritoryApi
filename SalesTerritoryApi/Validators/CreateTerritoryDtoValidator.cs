using System.Text.RegularExpressions;
using FluentValidation;
using SalesTerritoryApi.Models.DTOs;

namespace SalesTerritoryApi.Validators
{
    public class CreateTerritoryDtoValidator : AbstractValidator<CreateTerritoryDto>
    {
        public CreateTerritoryDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must be less than 100 characters");

            RuleFor(dto => dto.ZipCodes)
                .NotEmpty()
                .WithMessage("At least one zip code is required");

            RuleForEach(dto => dto.ZipCodes)
                .Matches(@"^\d{5}$")
                .WithMessage("{PropertyValue} is not a valid 5-digit zip code");

            RuleFor(dto => dto.Demographics)
                .ValidateDemographics();
        }
    }

    static partial class CreateTerritoryDtoValidationExtensions
    {
        public static IRuleBuilder<CreateTerritoryDto, Dictionary<string, object>> ValidateDemographics(this IRuleBuilder<CreateTerritoryDto, Dictionary<string, object>> builder)
        {
            return builder.Custom((demographics, context) =>
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

        [GeneratedRegex(@"^\d{5}$")]
        private static partial Regex MyRegex();
    }
}
