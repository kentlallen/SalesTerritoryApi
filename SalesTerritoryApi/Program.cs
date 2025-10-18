using Microsoft.EntityFrameworkCore;
using SalesTerritoryApi;

var builder = WebApplication.CreateBuilder(args);

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SalesDatabase");

// Register the DbContext and configure it to use PostgreSQL.
builder.Services.AddDbContext<TerritoryDbContext>(opt =>
    opt.UseNpgsql(connectionString));

// Register the Repository: When a controller asks for an ITerritoryRepository,
// provide an instance of EfTerritoryRepository.
builder.Services.AddScoped<ITerritoryRepository, EfTerritoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
