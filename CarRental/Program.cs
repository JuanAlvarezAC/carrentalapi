using CarRental.Data;
using Microsoft.EntityFrameworkCore;
using CarRentalServices.Contracts;
using CarRentalServices.Repositories;
using CarRentalServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Leer la conexión configurada en appsettings.json.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la conexión DefaultConnection.");

// Registrar el contexto de Entity Framework para MySQL.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

// Servicios y repositorios existentes.
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IMySqlDataService, MySqlDataService>();
builder.Services.AddKeyedScoped<ICategoryRepository, MySqlCategoryRepository>("MySql");
builder.Services.AddKeyedScoped<ICategoryRepository, CategoryRepository>("MsSql");
builder.Services.AddKeyedScoped<ICarsRepository, CarRepository>("MsSql");
builder.Services.AddKeyedScoped<ICarsRepository, CarsRepository>("MySql");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddKeyedScoped<IColorRepository, ColorRepository>("MySql");
builder.Services.AddKeyedScoped<IBrandRepository, BrandRepository>("MySql");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();