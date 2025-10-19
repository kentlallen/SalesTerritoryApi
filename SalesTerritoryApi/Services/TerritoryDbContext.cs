using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using SalesTerritoryApi.Models;

namespace SalesTerritoryApi.Services
{
    public class TerritoryDbContext : DbContext
    {
        public TerritoryDbContext(DbContextOptions<TerritoryDbContext> options) : base(options) { }

        public DbSet<SalesTerritory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This tells EF Core how to perform a deep comparison of the dictionary
            // to detect if its contents have actually changed.
            var dictionaryComparer = new ValueComparer<Dictionary<string, object>>(
                (d1, d2) => JsonSerializer.Serialize(d1, (JsonSerializerOptions?)null) == JsonSerializer.Serialize(d2, (JsonSerializerOptions?)null),
                d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                d => new Dictionary<string, object>(d)
            );

            // This configuration tells EF Core how to handle the Dictionary<string, object>.
            // It will serialize it to a JSON string when saving to the database and
            // deserialize it back into a Dictionary when reading from the database.
            modelBuilder.Entity<SalesTerritory>()
                .Property(t => t.Demographics)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, object>()
                )
                .Metadata.SetValueComparer(dictionaryComparer);
        }
    }
}
