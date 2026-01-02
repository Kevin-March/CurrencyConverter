using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using Domain.Entities;
using Application.Users.Commands;
using Application.Users.Queries;
using Application.Addresses.Commands;
using Application.Addresses.Queries;
using Application.Currencies.Commands;
using Application.CurrencyConversion;
using Security;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
// -------------------- Configuración básica --------------------
var builder = WebApplication.CreateBuilder(args);

// Leer API Key desde .env o appsettings.json
var apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? "default_api_key";

// Agregar DbContext SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Data Source=app.db"));

// Agregar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// -------------------- Middleware API Key --------------------
app.UseMiddleware<ApiKeyMiddleware>();

// -------------------- ENDPOINTS USERS --------------------

// Crear usuario
app.MapPost("/users", async (
    CreateUserCommand command,
    IValidator<CreateUserCommand> validator,
    AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    // Email único
    if (await db.Users.AnyAsync(u => u.Email == command.Email))
        return Results.BadRequest(new { message = "Email already exists." });

    var user = new User
    {
        Name = command.Name,
        Email = command.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(command.Password)
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

// Listar usuarios
app.MapGet("/users", async (bool? isActive, AppDbContext db) =>
{
    var query = db.Users.AsQueryable();
    if (isActive.HasValue) query = query.Where(u => u.IsActive == isActive.Value);
    var users = await query.ToListAsync();
    return Results.Ok(users);
});

// Obtener usuario por Id
app.MapGet("/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound(new { message = "User not found" });
});

// Modificar usuario
app.MapPut("/users/{id}", async (int id, [FromBody] UpdateUserCommand command, IValidator<UpdateUserCommand> validator, AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound(new { message = "User not found" });

    user.Name = command.Name;
    user.Email = command.Email;
    user.IsActive = command.IsActive;

    await db.SaveChangesAsync();
    return Results.Ok(user);
});

// Eliminar usuario
app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound(new { message = "User not found" });

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// -------------------- ENDPOINTS ADDRESSES --------------------

// Crear dirección para un usuario
app.MapPost("/users/{userId}/addresses", async (
    int userId,
    CreateAddressCommand command,
    IValidator<CreateAddressCommand> validator,
    AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    var user = await db.Users.FindAsync(userId);
    if (user is null) return Results.NotFound(new { message = "User not found" });

    var address = new Address
    {
        UserId = userId,
        Street = command.Street,
        City = command.City,
        Country = command.Country,
        ZipCode = command.ZipCode
    };

    db.Addresses.Add(address);
    await db.SaveChangesAsync();

    return Results.Created($"/addresses/{address.Id}", address);
});

// Listar direcciones de un usuario
app.MapGet("/users/{userId}/addresses", async (int userId, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(userId);
    if (user is null) return Results.NotFound(new { message = "User not found" });

    var addresses = await db.Addresses.Where(a => a.UserId == userId).Select(a => new
    {
        a.Id,
        a.Street,
        a.City,
        a.Country,
        a.ZipCode
    }).ToListAsync();
    return Results.Ok(addresses);
});

// Modificar dirección
app.MapPut("/addresses/{id}", async (
    int id,
    UpdateAddressCommand command,
    IValidator<UpdateAddressCommand> validator,
    AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    var address = await db.Addresses.FindAsync(id);
    if (address is null) return Results.NotFound(new { message = "Address not found" });

    address.Street = command.Street;
    address.City = command.City;
    address.Country = command.Country;
    address.ZipCode = command.ZipCode;

    await db.SaveChangesAsync();
    return Results.Ok(address);
});

// Eliminar dirección
app.MapDelete("/addresses/{id}", async (int id, AppDbContext db) =>
{
    var address = await db.Addresses.FindAsync(id);
    if (address is null) return Results.NotFound(new { message = "Address not found" });

    db.Addresses.Remove(address);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// -------------------- ENDPOINTS CURRENCIES --------------------

// Listar monedas
app.MapGet("/currencies", async (AppDbContext db) =>
{
    var currencies = await db.Currencies.ToListAsync();
    return Results.Ok(currencies);
});

// Crear moneda
app.MapPost("/currencies", async (
    CreateCurrencyCommand command,
    IValidator<CreateCurrencyCommand> validator,
    AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    if (await db.Currencies.AnyAsync(c => c.Code == command.Code))
        return Results.BadRequest(new { message = "Currency code already exists." });

    var currency = new Currency
    {
        Code = command.Code,
        Name = command.Name,
        RateToBase = command.RateToBase
    };

    db.Currencies.Add(currency);
    await db.SaveChangesAsync();

    return Results.Created($"/currencies/{currency.Id}", currency);
});

// Conversión de divisas
app.MapPost("/currency/convert", async (
    ConvertCurrencyCommand command,
    IValidator<ConvertCurrencyCommand> validator,
    AppDbContext db) =>
{
    var validation = validator.Validate(command);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);

    var from = await db.Currencies.FirstOrDefaultAsync(c => c.Code == command.FromCurrencyCode);
    var to = await db.Currencies.FirstOrDefaultAsync(c => c.Code == command.ToCurrencyCode);

    if (from is null || to is null)
        return Results.NotFound(new { message = "One or both currencies not found." });

    decimal amountInBase = command.Amount * from.RateToBase;
    decimal convertedAmount = amountInBase / to.RateToBase;

    return Results.Ok(new
    {
        fromCurrency = from.Code,
        toCurrency = to.Code,
        originalAmount = command.Amount,
        convertedAmount
    });
});

app.Run();