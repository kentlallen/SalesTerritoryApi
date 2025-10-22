using System.ComponentModel.DataAnnotations;

namespace SalesTerritoryApi.Models.DTOs
{
    public class CreateTerritoryDto
    {
        // Validation rules are applied in the ValidationFilterAttribute, so we don't validate with DataAnnotations
        public string Name { get; set; } = string.Empty;

        public List<string> ZipCodes { get; set; } = new();

        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
