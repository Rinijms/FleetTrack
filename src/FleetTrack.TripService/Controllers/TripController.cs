using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories;
using FleetTrack.TripService.Services;

using Microsoft.AspNetCore.Mvc;

namespace FleetTrack.TripService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _repo;
        private readonly ITripAssignmentService _assignmentService;

        public TripController(ITripRepository repo, ITripAssignmentService assignmentService)
        {
            _repo = repo;
            _assignmentService = assignmentService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{tripcode}")]
        public IActionResult GetByTripCode(string tripcode)
        {
            var trip = _repo.GetByTripCode(tripcode);
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpPost]
        public IActionResult CreateTrip([FromBody] CreateTrip createTrip)
        {
            var trip = new Trip{
                VehicleCode = createTrip.VehicleCode,
                StartLocation =createTrip.StartLocation,
                EndLocation = createTrip.EndLocation
            };

            var created = _repo.Add(trip);
            return CreatedAtAction(nameof(GetByTripCode), new { tripCode = created.TripCode }, created);
        }

        [HttpPost("{tripCode}/assign-driver")]
        public IActionResult AssignDriver(string tripCode, [FromQuery] string driverCode)
        {
            if (string.IsNullOrWhiteSpace(driverCode))
                return BadRequest("DriverCode is required.");

            var result = _repo.AssignDriver(tripCode, driverCode);

            if (result==null)
                return NotFound("Trip not found.");

            return Ok(new { message = "Driver assigned successfully." });
        }

        [HttpPost("{tripCode}/assign")]
        public async Task<IActionResult> AssignDriver(string tripCode, [FromBody] AssignDriverRequest req)
        {
            try
            {
                var result = await _assignmentService.AssignDriverAsync(tripCode, req.DriverCode);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
