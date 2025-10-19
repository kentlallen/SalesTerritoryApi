using Microsoft.EntityFrameworkCore;

namespace SalesTerritoryApi
{
    public class TerritoryDbContext : DbContext
    {
        public TerritoryDbContext(DbContextOptions<TerritoryDbContext> options) : base(options) { }

        public DbSet<SalesTerritory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This configuration tells EF Core how to handle the Dictionary<string, object>.
            // It will serialize it to a JSON string when saving to the database and
            // deserialize it back into a Dictionary when reading from the database.
            modelBuilder.Entity<SalesTerritory>()
                .Property(t => t.Demographics)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new Dictionary<string, object>()
                );
        }
    }
}
