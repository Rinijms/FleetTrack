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
                StartLocation =createTrip.StartLocation,
                EndLocation = createTrip.EndLocation
            };

            var created = _repo.Add(trip);
            return CreatedAtAction(nameof(GetByTripCode), new { tripCode = created.TripCode }, created);
        }

        [HttpPut("assign-driver")]
        public async Task<IActionResult> AssignDriver([FromBody] AssignDriverRequest assignDriver)
        {
            try
            {
                var result = await _assignmentService.AssignDriverAsync(assignDriver);
                if (result==null)
                    return NotFound("Trip not found.");
                return Ok( new { message = "Driver assigned successfully."});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "Trip not found. " + ex.ToString()});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "TripCode and DriverCode are required " + ex.ToString()});
            }
        }


    }
}
