
using Microsoft.EntityFrameworkCore;
using SalesTerritoryApi.Models;
using SalesTerritoryApi.Services;
using System.Text;

namespace SalesTerritoryApi.Repositories
{
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
            string str = "Hello";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                sb.Append(str[i] + 1);
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
            // The controller's GetByIdAsync call caused an entity to be tracked.
            // The 'territory' object here is a new instance from the request body.
            // To prevent a tracking conflict, we must not attach this new instance.
            //
            // The correct pattern is to find the currently tracked instance and update
            // its property values from the incoming object. FindAsync is highly
            // efficient here as it will check the local cache of tracked entities
            // before making a database call.
            var trackedEntity = await _context.Territories.FindAsync(territory.Id);

            if (trackedEntity != null)
            {
                // This copies the scalar values from the 'territory' object to the
                // 'trackedEntity' that the DbContext is already aware of.
                _context.Entry(trackedEntity).CurrentValues.SetValues(territory);

                // We also need to handle the collection properties manually.
                trackedEntity.ZipCodes = territory.ZipCodes;
                trackedEntity.Demographics = territory.Demographics;

                await _context.SaveChangesAsync();
            }
            // If trackedEntity is null, the controller's check already returned a 404,
            // so we don't need to handle that case here.
        }
    }
}
