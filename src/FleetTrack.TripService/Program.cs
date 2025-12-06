using Microsoft.EntityFrameworkCore;
using FleetTrack.TripService.Data;
using FleetTrack.TripService.Services;
using FleetTrack.TripService.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Repository with EFCore
builder.Services.AddScoped<ITripRepository,EFTripRepository>();

//DriverServiceURL
var driverServiceUrl = builder.Configuration["DriverService:BaseUrl"]
?? throw new InvalidOperationException("DriverService:BaseUrl is missing in appsettings.json");
builder.Services.AddHttpClient<IDriverClient, DriverClient>(c =>
{
    c.BaseAddress = new Uri(driverServiceUrl);
});

//VehicleServiceURL
var vehicleServiceUrl = builder.Configuration["VehicleService:BaseUrl"]
?? throw new InvalidOperationException("VehicleService:BaseUrl is missing in appsettings.json");
builder.Services.AddHttpClient<IVehicleClient, VehicleClient>(c =>
{
    c.BaseAddress = new Uri(vehicleServiceUrl);
});
 
builder.Services.AddScoped<ITripAssignmentService, TripAssignmentService>();

builder.Services.AddDbContext<TripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TripDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();