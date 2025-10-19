using SalesTerritoryApi.Models;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Models.ViewModels;

namespace SalesTerritoryApi.Services.Interfaces
{
    public interface ITerritoryService
    {
        Task<IEnumerable<TerritoryViewModel>> GetAllAsync();
        Task<TerritoryViewModel?> GetByIdAsync(int id);
        Task<TerritoryViewModel> CreateAsync(CreateTerritoryDto dto);
        Task<TerritoryViewModel?> UpdateAsync(int id, UpdateTerritoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
