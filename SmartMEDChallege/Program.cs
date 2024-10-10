using Microsoft.EntityFrameworkCore;
using SmartMEDChallege.Data;
using SmartMEDChallege.Interfaces;
using SmartMEDChallege.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IMedicationService, MedicationService>();

//Use in memory DB
builder.Services.AddDbContext<MedicationDbContext>(options =>
	options.UseInMemoryDatabase("MedicationDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
