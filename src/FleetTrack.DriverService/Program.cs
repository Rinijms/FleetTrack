using Microsoft.EntityFrameworkCore;
using FleetTrack.DriverService.Repositories;
using FleetTrack.DriverService.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DriverDbContext> (options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DriverServiceDb")));

//Repository
builder.Services.AddScoped<IDriverRepository, EFDriverRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

