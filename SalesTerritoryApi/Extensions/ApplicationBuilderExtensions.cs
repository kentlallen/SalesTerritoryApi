using SalesTerritoryApi.Data;
using SalesTerritoryApi.Services;
using Microsoft.EntityFrameworkCore;

namespace SalesTerritoryApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TerritoryDbContext>();
            
            await context.Database.MigrateAsync();
            
            // Seed the database with sample data
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
            
            return app;
        }
    }
}
