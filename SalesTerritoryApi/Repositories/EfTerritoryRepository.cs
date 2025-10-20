
using Microsoft.EntityFrameworkCore;
using SalesTerritoryApi.Models;
using SalesTerritoryApi.Services;
using SalesTerritoryApi.Data;
using System.Text;

namespace SalesTerritoryApi.Repositories
{
    // Repository pattern implementation - abstracts data access from the service layer
    public class EfTerritoryRepository(TerritoryDbContext _context) : ITerritoryRepository
    {
        public async Task<SalesTerritory> CreateAsync(SalesTerritory territory)
        {
            _context.Territories.Add(territory);
            await _context.SaveChangesAsync();
            return territory;
        }

        public async Task DeleteAsync(int id)
        {
            var territory = await _context.Territories.FindAsync(id);
            if (territory != null)
            {
                _context.Territories.Remove(territory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SalesTerritory>> GetAllAsync()
        {
            return await _context.Territories.ToListAsync();
        }

        public async Task<SalesTerritory?> GetByIdAsync(int id)
        {
            return await _context.Territories.FindAsync(id);
        }

        public async Task UpdateAsync(SalesTerritory territory)
        {
            // EF Core tracking issue: the incoming 'territory' is a new instance from the request body
            // but we might already have a tracked entity from the controller's GetByIdAsync call.
            // Using FindAsync is efficient - it checks the local cache first before hitting the DB.
            var trackedEntity = await _context.Territories.FindAsync(territory.Id);

            if (trackedEntity != null)
            {
                // Copy scalar properties from the incoming object to the tracked entity
                _context.Entry(trackedEntity).CurrentValues.SetValues(territory);

                // Collections need to be handled manually since SetValues doesn't copy them
                trackedEntity.ZipCodes = territory.ZipCodes;
                trackedEntity.Demographics = territory.Demographics;

                await _context.SaveChangesAsync();
            }
            // If trackedEntity is null, the service layer already handled the 404 case
        }
    }
}
