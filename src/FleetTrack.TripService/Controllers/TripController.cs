using FleetTrack.TripService.Models;
using FleetTrack.TripService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FleetTrack.TripService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _repo;

        public TripController(ITripRepository repo)
        {
            _repo = repo;
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
        public IActionResult CreateTrip([FromBody] Trip trip)
        {
            var created = _repo.Add(trip);
            return CreatedAtAction(nameof(GetByTripCode), new { tripCode = created.TripCode }, created);
        }
    }
}
