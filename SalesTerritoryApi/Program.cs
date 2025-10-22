using SalesTerritoryApi.Extensions;
using SalesTerritoryApi.Middleware;
using SalesTerritoryApi.Data;
using SalesTerritoryApi.Configuration;
using Serilog;
using Serilog.Debugging;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// Enable Serilog's internal logging to console for debugging
SelfLog.Enable(Console.Error);

// Set up bootstrap logger - this runs before the main DI container is ready
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up the Sales Territory API");

// Ensure PostgreSQL container is running
await EnsurePostgreSQLContainerAsync();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Replace the default logger with Serilog for structured logging
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Pull Serilog config from appsettings.json
        .ReadFrom.Services(services)                   // Enable DI services in logging (like ILogger<T>)
        .Enrich.FromLogContext());                     // Add request context to all log entries

    const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // CORS setup for the React frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins(Ports.ReactUrl) // React dev server
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // Core ASP.NET services
    builder.Services.AddControllers(options =>
    {
        // Add the validation filter to all controller actions
        options.Filters.Add<ValidationFilterAttribute>();
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Health checks for monitoring and load balancers
    builder.Services.AddHealthChecks();

    // Add problem details middleware for consistent error responses
    builder.Services.AddProblemDetails();
    // Configure the API behavior options to return a 422 Unprocessable Entity response for invalid model state
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status422UnprocessableEntity
            };
            return new UnprocessableEntityObjectResult(problemDetails);
        };
    });

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

// Helper method to ensure PostgreSQL container is running
static async Task EnsurePostgreSQLContainerAsync()
{
    try
    {
        // Check if the container is already running
        var checkProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "ps --filter name=salesterritory-postgres --filter status=running --format \"{{.Names}}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        checkProcess.Start();
        await checkProcess.WaitForExitAsync();
        var output = await checkProcess.StandardOutput.ReadToEndAsync();

        if (output.Contains("salesterritory-postgres"))
        {
            Log.Information("PostgreSQL container is already running");
            return;
        }

        Log.Information("PostgreSQL container not found, starting it...");

        // Start the container using docker-compose
        var startProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments = "up -d postgres",
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        startProcess.Start();
        await startProcess.WaitForExitAsync();

        if (startProcess.ExitCode == 0)
        {
            Log.Information("PostgreSQL container started successfully");
            
            // Wait a moment for the container to be ready
            await Task.Delay(3000);
        }
        else
        {
            var error = await startProcess.StandardError.ReadToEndAsync();
            Log.Warning("Failed to start PostgreSQL container: {Error}", error);
            Log.Warning("Please ensure Docker is installed and running, or start the container manually with: docker-compose up -d postgres");
        }
    }
    catch (Exception ex)
    {
        Log.Warning("Could not check/start PostgreSQL container: {Error}", ex.Message);
        Log.Warning("Please ensure Docker is installed and running, or start the container manually with: docker-compose up -d postgres");
    }
}
