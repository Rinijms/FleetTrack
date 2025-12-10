using FleetTrack.VehicleLocationService.Data;
using FleetTrack.VehicleLocationService.Repositories;
using FleetTrack.VehicleLocationService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration & Logging already present
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        // safe defaults
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DB
var conn = builder.Configuration.GetConnectionString("VehicleLocationDb");
builder.Services.AddDbContext<VehicleLocationDbContext>(options =>
    options.UseSqlServer(conn));

// DI - Register repository(EF) & Services
builder.Services.AddScoped<ILocationRepository, EFLocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();