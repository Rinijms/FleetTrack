using FleetTrack.VehicleService.Repositories;
using Microsoft.EntityFrameworkCore; 
using FleetTrack.VehicleService.Data;
using FleetTrack.VehicleService.Messaging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<TripCompletedConsumer>();
builder.Services.AddScoped<IVehicleRepository,EFVehicleRepository>();

builder.Services.AddDbContext<VehicleDbContext> (options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleServiceDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); 
app.Run();