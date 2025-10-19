using SalesTerritoryApi.Models;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Models.ViewModels;
using SalesTerritoryApi.Services.Interfaces;
using SalesTerritoryApi.Repositories;

namespace SalesTerritoryApi.Services
{
    public class TerritoryService : ITerritoryService
    {
        private readonly ITerritoryRepository _repository;

        public TerritoryService(ITerritoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TerritoryViewModel>> GetAllAsync()
        {
            var territories = await _repository.GetAllAsync();
            return territories.Select(MapToViewModel);
        }

        public async Task<TerritoryViewModel?> GetByIdAsync(int id)
        {
            var territory = await _repository.GetByIdAsync(id);
            return territory != null ? MapToViewModel(territory) : null;
        }

        public async Task<TerritoryViewModel> CreateAsync(CreateTerritoryDto dto)
        {
            var territory = new SalesTerritory
            {
                Name = dto.Name,
                ZipCodes = dto.ZipCodes,
                Demographics = dto.Demographics
            };

            var createdTerritory = await _repository.CreateAsync(territory);
            return MapToViewModel(createdTerritory);
        }

        public async Task<TerritoryViewModel?> UpdateAsync(int id, UpdateTerritoryDto dto)
        {
            var existingTerritory = await _repository.GetByIdAsync(id);
            if (existingTerritory == null)
                return null;

            existingTerritory.Name = dto.Name;
            existingTerritory.ZipCodes = dto.ZipCodes;
            existingTerritory.Demographics = dto.Demographics;

            await _repository.UpdateAsync(existingTerritory);
            return MapToViewModel(existingTerritory);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingTerritory = await _repository.GetByIdAsync(id);
            if (existingTerritory == null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        private static TerritoryViewModel MapToViewModel(SalesTerritory territory)
        {
            return new TerritoryViewModel
            {
                Id = territory.Id,
                Name = territory.Name,
                ZipCodes = territory.ZipCodes,
                Demographics = territory.Demographics
            };
        }
    }
}
