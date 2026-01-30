using gestion_beneficiarios.Context;
using Microsoft.EntityFrameworkCore;
using gestion_beneficiarios.Repositories;
using gestion_beneficiarios.Repositories.Interfaces;
using gestion_beneficiarios.Services;
using gestion_beneficiarios.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Variables de entorno
var server = builder.Configuration["DB_SERVER"];
var database = builder.Configuration["DB_NAME"];
var user = builder.Configuration["DB_USER"];
var password = builder.Configuration["DB_PASSWORD"];

// Construcción de la cadena de conexión
var connectionString =
    $"Server={server};" +
    $"Database={database};" +
    $"User Id={user};" +
    $"Password={password};" +
    $"TrustServerCertificate=True;";

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// REGISTRO DE DEPENDENCIAS
builder.Services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
builder.Services.AddScoped<IIdentityDocumentRepository, IdentityDocumentRepository>();
builder.Services.AddScoped<IIdentityDocumentService, IdentityDocumentService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();


Console.WriteLine($"DB Server: {server}");

app.Run();