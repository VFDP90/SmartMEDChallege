using Microsoft.EntityFrameworkCore;
using SmartMEDChallege.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Use in memory DB
builder.Services.AddDbContext<MedicationDbContext>(options =>
	options.UseInMemoryDatabase("MedicationDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
