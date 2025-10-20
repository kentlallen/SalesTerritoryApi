using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesTerritoryApi.Models
{
    public class SalesTerritory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<string> ZipCodes { get; set; } = new();

        // Flexible JSON storage for demographic data - EF Core maps this to JSONB in PostgreSQL
        // This allows us to store varying demographic structures without schema changes
        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
