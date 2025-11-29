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

builder.Services.AddHttpClient<IDriverClient, DriverClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5009"); // DriverService URL
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