using SalesTerritoryApi.Data;
using SalesTerritoryApi.Models;
using SalesTerritoryApi.Repositories;
using SalesTerritoryApi.Services;
using SalesTerritoryApi.Services.Interfaces;
using SalesTerritoryApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SalesTerritoryApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository pattern
            services.AddScoped<ITerritoryRepository, EfTerritoryRepository>();
            
            // Service layer
            services.AddScoped<ITerritoryService, TerritoryService>();
            
            // Validation
            services.AddScoped<IValidator<SalesTerritory>, SalesTerritoryValidator>();
            
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TerritoryDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SalesDatabase")));

            return services;
        }
    }
}
