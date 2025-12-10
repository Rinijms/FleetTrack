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
 
    [HttpPut("UpdateStatus")]     
    public IActionResult UpdateStatus(UpdateVehicleStatusDTO updateDTO)
    { 
        var ok = _repo.UpdateStatus(updateDTO);          
        return NoContent();
    }

    [HttpGet("{vehicleCode}/history")]
    public IActionResult GetHistory(string vehicleCode)
    {
        var history = _repo.GetHistory(vehicleCode);
        return Ok(history);
    }
}