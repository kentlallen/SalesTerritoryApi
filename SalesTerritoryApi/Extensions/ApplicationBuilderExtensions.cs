using SalesTerritoryApi.Data;
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
            
            return app;
        }
    }
}
