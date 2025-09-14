// backend/Api/Program.cs
using Application.Services;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Injeção de dependência
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints CRUD
app.MapPost("/produtos", async (ProductService svc, ProductDto dto) =>
{
    var id = await svc.CreateAsync(dto.Name, dto.Price);
    return Results.Created($"/produtos/{id}", new { id });
});

app.MapGet("/produtos", async (ProductService svc) =>
    Results.Ok(await svc.ListAsync()));

app.MapGet("/produtos/{id:int}", async (ProductService svc, int id) =>
{
    var p = await svc.GetAsync(id);
    return p is null ? Results.NotFound() : Results.Ok(p);
});

app.MapPut("/produtos/{id:int}", async (ProductService svc, int id, ProductDto dto) =>
{
    await svc.UpdateAsync(id, dto.Name, dto.Price);
    return Results.NoContent();
});

app.MapDelete("/produtos/{id:int}", async (ProductService svc, int id) =>
{
    await svc.DeleteAsync(id);
    return Results.NoContent();
});

// SignalR Hub
app.MapHub<ProductHub>("/hubs/product");

app.Run();

record ProductDto(string Name, decimal Price);

public class ProductHub : Microsoft.AspNetCore.SignalR.Hub { }
