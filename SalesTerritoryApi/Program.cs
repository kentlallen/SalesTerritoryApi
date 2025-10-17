using Microsoft.EntityFrameworkCore;
using SalesTerritoryApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext and configure it to use an in-memory database.
builder.Services.AddDbContext<TerritoryDbContext>(opt => opt.UseInMemoryDatabase("SalesDB"));
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

app.MapControllers();

app.Run();
