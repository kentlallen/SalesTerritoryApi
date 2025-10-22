using System.ComponentModel.DataAnnotations;

namespace SalesTerritoryApi.Models.DTOs
{
    public class UpdateTerritoryDto
    {
        // Validation rules are applied in the ValidationFilterAttribute, so we don't need to validate with DataAnnotations
        public string Name { get; set; } = string.Empty;

        public List<string> ZipCodes { get; set; } = new();

        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
