using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesTerritoryApi
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

        // This flexible dictionary mimics a JSON object, perfect for storing
        // varied demographic data from external sources. In a real Postgres DB,
        // this would be mapped to a JSONB column.
        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
