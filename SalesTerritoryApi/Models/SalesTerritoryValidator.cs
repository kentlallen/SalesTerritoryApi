using System.Text.RegularExpressions;
using FluentValidation;

namespace SalesTerritoryApi.Models
{
    class SalesTerritoryValidator : AbstractValidator<SalesTerritory>
    {
        public SalesTerritoryValidator()
        {
            RuleFor(territory => territory.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name must be less than 100 characters");
            RuleFor(territory => territory.ZipCodes).NotEmpty().WithMessage("At least one zip code is required");
            RuleForEach(territory => territory.ZipCodes).Matches(@"^\d{5}$").WithMessage("{PropertyValue} is not a valid 5-digit zip code");
            RuleFor(territory => territory.Demographics).NotEmpty().WithMessage("Demographics are required in the form {\"Name1\":\"Data1\", \"Name2\":\"Data2\"}").ValidateDemographics();
        }
    }

    static partial class ValidationExtensions
    {
        public static IRuleBuilder<SalesTerritory, List<string>> ValidateZipCodes(this IRuleBuilder<SalesTerritory, List<string>> builder)
        {
            return builder.Custom((zipCodes, context) =>
            {
                foreach (var zipCode in zipCodes)
                {
                    if (!MyRegex().IsMatch(zipCode))
                    {
                        context.AddFailure($"{zipCode}", "is not a valid 5-digit zip code");
                    }
                }
            });
        }

        public static IRuleBuilder<SalesTerritory, Dictionary<string, object>> ValidateDemographics(this IRuleBuilder<SalesTerritory, Dictionary<string, object>> builder)
        {
            return builder.Custom((demographics, context) =>
            {
                foreach (var demographic in demographics)
                {
                    if (string.IsNullOrEmpty(demographic.Key)|| string.IsNullOrEmpty(demographic.Value.ToString()))
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