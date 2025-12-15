using FleetTrack.Shared.EventBus;
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
        private readonly IEventBusProducer _eventBus;

        public TripController(ITripRepository repo, ITripAssignmentService assignmentService,IEventBusProducer eventBus)
        {
            _repo = repo;
            _assignmentService = assignmentService;
            _eventBus=eventBus;
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
                EndLocation = createTrip.EndLocation,
                StartTime= DateTime.UtcNow
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

        [HttpPut("assign-vehicle")]
        public async Task<IActionResult> AssignVehicle([FromBody] AssignVehicleRequest assignVehicle)
        {
            try
            {
                var result = await _assignmentService.AssignVehicleAsync(assignVehicle);
                if (result == null)
                    return NotFound("Trip not found.");

                return Ok(new { message = "Vehicle assigned successfully." });
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
    
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteTrip([FromBody]CompleteTripRequest completeTrip)
        {
             try
            {
                var result = await _assignmentService.CompletedTrip(completeTrip);
                if (result == null)
                    return NotFound(new { message ="Trip not found."});

                return Ok(new { message = "Trip completed successfully." });
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
