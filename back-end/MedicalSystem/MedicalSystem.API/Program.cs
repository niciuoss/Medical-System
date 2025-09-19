using Microsoft.EntityFrameworkCore;
using MedicalSystem.Infrastructure.Data;
using MedicalSystem.Domain.Entities;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Infrastructure.Repositories;
using MedicalSystem.Application.Services.Interfaces;
using MedicalSystem.Application.Services;
using MedicalSystem.Application.Mappings;
using MedicalSystem.Application.Validators;
using MedicalSystem.Application.DTOs.Patient;
using MedicalSystem.Application.DTOs.MedicalReport;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<MedicalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add FluentValidation
builder.Services.AddScoped<IValidator<CreatePatientDto>, CreatePatientValidator>();
builder.Services.AddScoped<IValidator<UpdatePatientDto>, UpdatePatientValidator>();
builder.Services.AddScoped<IValidator<CreateMedicalReportDto>, CreateMedicalReportValidator>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicalReportRepository, MedicalReportRepository>();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalReportService, MedicalReportService>();

// Add password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
var context = scope.ServiceProvider.GetRequiredService<MedicalDbContext>();
var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
await DbSeeder.SeedAsync(context, passwordHasher);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Manter o WeatherForecast temporariamente
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
var forecast = Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    ))
    .ToArray();
return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}