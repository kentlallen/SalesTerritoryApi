using SalesTerritoryApi.Extensions;
using SalesTerritoryApi.Middleware;
using SalesTerritoryApi.Data;
using Serilog;
using Serilog.Debugging;

// Enable Serilog's internal logging to console for debugging
SelfLog.Enable(Console.Error);

// Set up bootstrap logger - this runs before the main DI container is ready
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up the Sales Territory API");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Replace the default logger with Serilog for structured logging
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Pull Serilog config from appsettings.json
        .ReadFrom.Services(services)                   // Enable DI services in logging (like ILogger<T>)
        .Enrich.FromLogContext());                     // Add request context to all log entries

    const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // CORS setup for the React frontend running on port 5173
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:5173") // React dev server
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // Core ASP.NET services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Health checks for monitoring and load balancers
    builder.Services.AddHealthChecks();

    // Custom service registration using extension methods (keeps Program.cs clean)
    builder.Services.AddApplicationServices(); // Repository, Service, and Validation registrations
    builder.Services.AddDatabase(builder.Configuration); // EF Core DbContext setup


    var app = builder.Build();

    // Enable Serilog request logging middleware
    app.UseSerilogRequestLogging();

    // Swagger only in development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Custom global exception handler (must be early in pipeline)
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Standard ASP.NET middleware pipeline
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors(MyAllowSpecificOrigins);

    app.MapControllers();

    // Health check endpoints for monitoring
    app.MapHealthChecks("/health"); // Basic health check
    app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready") // Readiness probe for Kubernetes
    });
    app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        Predicate = _ => false // Liveness probe (always returns healthy)
    });

    // Auto-migrate database on startup (useful for development)
    await app.MigrateDatabaseAsync();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down the Sales Territory API");
    Log.CloseAndFlush();
}
