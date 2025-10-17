namespace SalesTerritoryApi
{
    public interface ITerritoryRepository
    {
        Task<IEnumerable<SalesTerritory>> GetAllAsync();
        Task<SalesTerritory?> GetByIdAsync(int id);
        Task<SalesTerritory> CreateAsync(SalesTerritory territory);
        Task UpdateAsync(SalesTerritory territory);
        Task DeleteAsync(int id);
    }
}
