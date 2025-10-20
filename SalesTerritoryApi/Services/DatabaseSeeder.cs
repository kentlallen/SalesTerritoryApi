using SalesTerritoryApi.Data;
using SalesTerritoryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SalesTerritoryApi.Services
{
    public class DatabaseSeeder
    {
        private readonly TerritoryDbContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(TerritoryDbContext context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding...");
                
                // Check if data already exists
                if (await _context.Territories.AnyAsync())
                {
                    _logger.LogInformation("Database already contains data, skipping seeding.");
                    return; // Database already has data
                }

            var sampleTerritories = new List<SalesTerritory>
            {
                new SalesTerritory
                {
                    Name = "Northwest Region",
                    ZipCodes = new List<string> { "98101", "98102", "98103", "98104", "98105" },
                    Demographics = new Dictionary<string, object>
                    {
                        { "population", 2500000 },
                        { "medianIncome", 75000 },
                        { "primaryIndustries", new List<string> { "Technology", "Healthcare", "Finance" } },
                        { "growthRate", 3.2 }
                    }
                },
                new SalesTerritory
                {
                    Name = "Southeast Region",
                    ZipCodes = new List<string> { "30301", "30302", "30303", "30304", "30305" },
                    Demographics = new Dictionary<string, object>
                    {
                        { "population", 1800000 },
                        { "medianIncome", 65000 },
                        { "primaryIndustries", new List<string> { "Manufacturing", "Logistics", "Healthcare" } },
                        { "growthRate", 2.8 }
                    }
                },
                new SalesTerritory
                {
                    Name = "Southwest Region",
                    ZipCodes = new List<string> { "85001", "85002", "85003", "85004", "85005" },
                    Demographics = new Dictionary<string, object>
                    {
                        { "population", 1200000 },
                        { "medianIncome", 58000 },
                        { "primaryIndustries", new List<string> { "Tourism", "Real Estate", "Technology" } },
                        { "growthRate", 4.1 }
                    }
                },
                new SalesTerritory
                {
                    Name = "Northeast Region",
                    ZipCodes = new List<string> { "02101", "02102", "02103", "02104", "02105" },
                    Demographics = new Dictionary<string, object>
                    {
                        { "population", 3200000 },
                        { "medianIncome", 82000 },
                        { "primaryIndustries", new List<string> { "Finance", "Education", "Biotech" } },
                        { "growthRate", 2.1 }
                    }
                },
                new SalesTerritory
                {
                    Name = "Central Region",
                    ZipCodes = new List<string> { "60601", "60602", "60603", "60604", "60605" },
                    Demographics = new Dictionary<string, object>
                    {
                        { "population", 2100000 },
                        { "medianIncome", 68000 },
                        { "primaryIndustries", new List<string> { "Manufacturing", "Transportation", "Finance" } },
                        { "growthRate", 1.9 }
                    }
                }
            };

                _context.Territories.AddRange(sampleTerritories);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Database seeding completed successfully. Added {Count} territories.", sampleTerritories.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during database seeding.");
                throw;
            }
        }
    }
}
