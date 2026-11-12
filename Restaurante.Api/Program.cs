using Microsoft.EntityFrameworkCore;
using Restaurante.Datos;
using Restaurante.Datos.Interfaces;
using Restaurante.Datos.Repositorios;
using Restaurante.Negocio.Interfaces;
using Restaurante.Negocio.Mapeos;
using Restaurante.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Servicios de la API
// ------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------------
// Entity Framework Core
// ------------------------------------------------------------
builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestauranteDB")));

// ------------------------------------------------------------
// Repositorios (capa de datos)
// ------------------------------------------------------------
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();

// ------------------------------------------------------------
// Servicios de negocio
// ------------------------------------------------------------
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IPlatoServicio, PlatoServicio>();

// ------------------------------------------------------------
// AutoMapper: un único perfil en la capa de Negocio
// ------------------------------------------------------------
builder.Services.AddAutoMapper(
    configuracion => { },
    typeof(MappingProfile).Assembly);

var app = builder.Build();

// ------------------------------------------------------------
// Pipeline HTTP
// ------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Restaurante v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
