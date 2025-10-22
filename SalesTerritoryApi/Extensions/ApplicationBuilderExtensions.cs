using SalesTerritoryApi.Data;
using SalesTerritoryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace SalesTerritoryApi.Extensions
{
    /// <summary>
    /// Extension methods for the WebApplication class
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Migrates the database and seeds the database with sample data if it is empty
        /// </summary>
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TerritoryDbContext>();
            
            await context.Database.MigrateAsync();
            
            // Seed the database with sample data if it is empty
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
            
            return app;
        }
    }
}
