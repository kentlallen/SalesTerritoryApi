using System.ComponentModel.DataAnnotations;

namespace SalesTerritoryApi.Models.DTOs
{
    public class UpdateTerritoryDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<string> ZipCodes { get; set; } = new();

        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
