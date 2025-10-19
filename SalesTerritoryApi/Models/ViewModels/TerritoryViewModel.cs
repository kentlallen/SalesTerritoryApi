namespace SalesTerritoryApi.Models.ViewModels
{
    public class TerritoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> ZipCodes { get; set; } = new();
        public Dictionary<string, object> Demographics { get; set; } = new();
    }
}
