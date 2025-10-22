using FluentValidation;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Validators.Shared;

namespace SalesTerritoryApi.Validators
{
    public class CreateTerritoryDtoValidator : AbstractValidator<CreateTerritoryDto>
    {
        public CreateTerritoryDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .ApplyNameValidation();

            RuleFor(dto => dto.ZipCodes)
                .ApplyZipCodeCollectionValidation();

            RuleFor(dto => dto.Demographics)
                .ApplyDemographicsValidation();
        }
    }
}
