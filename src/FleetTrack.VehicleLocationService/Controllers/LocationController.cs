using Microsoft.AspNetCore.Mvc;
using FleetTrack.VehicleLocationService.DTOs;
using FleetTrack.VehicleLocationService.Services;
using FleetTrack.VehicleLocationService.Repositories;
using FleetTrack.VehicleLocationService.Models;
using System.Threading.Tasks;

namespace FleetTrack.VehicleLocationService.Controllers
{

    [ApiController]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    { 
        private readonly ILocationService _service;

        public LocationController(ILocationService service) => _service = service;
       
        // POST /api/locations
        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] VehicleLocationUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _service.SaveLocationAsync(dto);
            return CreatedAtAction(nameof(GetLatest), new { vehicleCode = created.VehicleCode }, created);
           
        }

        // GET /api/locations
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int limit = 100)
        {
            var items = await _service.GetAllAsync(limit);
            return Ok(items);
        }

        [HttpGet("{vehicleCode}/latest")]
        public async Task<IActionResult> GetLatest(string vehicleCode)
        {
            var item = await _service.GetLatestAsync(vehicleCode);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("{vehicleCode}/history")]
        public async Task<IActionResult> GetHistory(string vehicleCode, [FromQuery] int take = 100)
        {
            var items = await _service.GetHistoryAsync(vehicleCode, take);
            return Ok(items);
        }
    }

}