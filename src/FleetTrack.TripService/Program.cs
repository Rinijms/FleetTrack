var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Repository
builder.Services.AddSingleton<FleetTrack.TripService.Repositories.ITripRepository,
FleetTrack.TripService.Repositories.InMemoryTripRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();