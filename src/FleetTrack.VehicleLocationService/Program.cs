var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { /* keep default behavior for now */ });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repository (in-memory for now)
builder.Services.AddSingleton<FleetTrack.VehicleLocationService.Repositories.ILocationRepository, 
FleetTrack.VehicleLocationService.Repositories.InMemoryLocationRepository>();

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