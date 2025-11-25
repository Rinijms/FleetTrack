using Microsoft.AspNetCore.Mvc;
using FleetTrack.VehicleLocationService.DTOs;
using FleetTrack.VehicleLocationService.Models;
using FleetTrack.VehicleLocationService.Repositories;

namespace FleetTrack.VehicleLocationService.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController : ControllerBase
{
    private readonly ILocationRepository _repo;
    private readonly ILogger<LocationController> _logger;

    public LocationController(ILocationRepository repo, ILogger<LocationController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    // POST /api/locations
    [HttpPost]
    public async Task<IActionResult> PostLocation([FromBody] VehicleLocationUpdateDTO dto, CancellationToken ct)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.VehicleId))
            return BadRequest(new { error = "vehicleId, latitude and longitude are required" });

        var record = new LocationRecord
        {
            VehicleId = dto.VehicleId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            TimestampUtc = DateTime.UtcNow
        };

        var added = await _repo.AddAsync(record, ct);

        _logger.LogInformation("Received location for {VehicleId} at {Lat},{Lon}", dto.VehicleId, dto.Latitude, dto.Longitude);

        // NOTE: later we will publish a LocationUpdated event to message broker here

        return CreatedAtAction(nameof(GetLocationsByVehicle), new { vehicleId = added.VehicleId }, added);
    }

    // GET /api/locations
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var all = await _repo.GetAllAsync(ct);
        return Ok(all);
    }

    // GET /api/locations/{vehicleId}
    [HttpGet("{vehicleId}")]
    public async Task<IActionResult> GetLocationsByVehicle(string vehicleId, CancellationToken ct)
    {
        var list = await _repo.GetByVehicleAsync(vehicleId, ct);
        if (!list.Any()) return NotFound(new { message = "No locations found for vehicle" });
        return Ok(list);
    }

    // GET /api/locations/health
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", time = DateTime.UtcNow });
}