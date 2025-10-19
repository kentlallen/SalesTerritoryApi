using SalesTerritoryApi.Extensions;
using SalesTerritoryApi.Middleware;
using Serilog;
using Serilog.Debugging;

// This will print any internal Serilog errors to the console.
SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up the Sales Territory API");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog service for better logging
    // This replaces the default logger with Serilog.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Reads config from appsettings.json
        .ReadFrom.Services(services)                   // Allows DI services to be used in logging
        .Enrich.FromLogContext());                     // Adds contextual information to logs

    const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // Add CORS policy so the API can accept requests from the React UI app.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add application services using extension methods
    builder.Services.AddApplicationServices();
    builder.Services.AddDatabase(builder.Configuration);


    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Add custom exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors(MyAllowSpecificOrigins);

    app.MapControllers();

    // Migrate database on startup
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
