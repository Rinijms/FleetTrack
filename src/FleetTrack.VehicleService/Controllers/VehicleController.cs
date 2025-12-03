using FleetTrack.VehicleService.Models;
using FleetTrack.VehicleService.DTOs;
using FleetTrack.VehicleService.Repositories;
using Microsoft.AspNetCore.Mvc;
using FleetTrack.VehicleService.Enums;

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
            Type = req.Type,
            VehicleCode = string.Empty,
            Status = VehicleStatus.Active
            // Id/VehicleCode/Status handled by repository
        };

        var created = _repo.Add(vehicle);

        return CreatedAtAction(nameof(GetByCode), new { vehicleCode = created.VehicleCode }, created);
    }

    [HttpPut("{vehicleCode}/status")]
    public IActionResult UpdateStatus(string vehicleCode, [FromBody] string newStatus)
    {
        if (!Enum.TryParse<VehicleStatus>(newStatus,true,out var newstatus))
         return BadRequest(new { error = "Invalid vehicle status" });

        var ok = _repo.UpdateStatus(vehicleCode, newstatus);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("{vehicleCode}/history")]
    public IActionResult GetHistory(string vehicleCode)
    {
        var history = _repo.GetHistory(vehicleCode);
        return Ok(history);
    }
}