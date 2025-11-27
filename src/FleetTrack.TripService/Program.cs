var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Repository
builder.Services.AddSingleton<FleetTrack.TripService.Repositories.ITripRepository,
FleetTrack.TripService.Repositories.InMemoryTripRepository>();

builder.Services.AddHttpClient<IDriverClient, DriverClient>(c =>
{
    c.BaseAddress = new Uri("http://localhost:5009"); // DriverService URL
});

builder.Services.AddScoped<FleetTrack.TripService.Services.ITripAssignmentService, FleetTrack.TripService.Services.TripAssignmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();