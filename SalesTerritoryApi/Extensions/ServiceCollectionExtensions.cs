using SalesTerritoryApi.Data;
using SalesTerritoryApi.Models;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Repositories;
using SalesTerritoryApi.Services;
using SalesTerritoryApi.Services.Interfaces;
using SalesTerritoryApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace SalesTerritoryApi.Extensions
{
    /// <summary>
    /// Extension methods for the IServiceCollection class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository pattern
            services.AddScoped<ITerritoryRepository, EfTerritoryRepository>();
            
            // Service layer
            services.AddScoped<ITerritoryService, TerritoryService>();
            
            // Validation
            services.AddScoped<IValidator<CreateTerritoryDto>, CreateTerritoryDtoValidator>();
            services.AddScoped<IValidator<UpdateTerritoryDto>, UpdateTerritoryDtoValidator>();
            
            // Database seeding
            services.AddScoped<DatabaseSeeder>();
            
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
