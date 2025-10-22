using FluentValidation;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Validators.Shared;

namespace SalesTerritoryApi.Validators
{
    public class UpdateTerritoryDtoValidator : AbstractValidator<UpdateTerritoryDto>
    {
        public UpdateTerritoryDtoValidator()
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
