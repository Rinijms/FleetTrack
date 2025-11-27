using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.DTOs;
using FleetTrack.VehicleService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FleetTrack.VehicleService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleRepository _repo;

    public VehicleController(IVehicleRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_repo.GetAll());

    [HttpGet("{vehicleCode}")]
    public IActionResult GetByCode(string vehicleCode)
    {
        var v = _repo.GetByVehicleCode(vehicleCode);
        if (v == null) return NotFound();
        return Ok(v);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateVehicleRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.RegistrationNumber) || string.IsNullOrWhiteSpace(req.Type))
            return BadRequest(new { error = "registrationNumber and type are required" });

        var vehicle = new Vehicle
        {
            RegistrationNumber = req.RegistrationNumber,
            Type = req.Type
            // Id/VehicleCode/Status handled by repository
        };

        var created = _repo.Add(vehicle);

        return CreatedAtAction(nameof(GetByCode), new { vehicleCode = created.VehicleCode }, created);
    }

    [HttpPut("{vehicleCode}/status")]
    public IActionResult UpdateStatus(string vehicleCode, [FromBody] string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus)) return BadRequest(new { error = "newStatus is required in body" });

        var ok = _repo.UpdateStatus(vehicleCode, newStatus);
        if (!ok) return NotFound();
        return NoContent();
    }
}