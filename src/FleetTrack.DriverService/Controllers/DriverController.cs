using FleetTrack.DriverService.Models;
using FleetTrack.DriverService.Repositories;
using Microsoft.AspNetCore.Mvc;
using FleetTrack.DriverService.Enums;

namespace FleetTrack.DriverService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _repo;

        public DriverController(IDriverRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{driverCode}")]
        public IActionResult GetByDriverCode(string driverCode)
        {
            var driver = _repo.GetByDriverCode(driverCode);
            if (driver == null)
                return NotFound();

            return Ok(driver);
        }

        [HttpPost]
        public IActionResult CreateDriver([FromBody] CreateDriverRequest request)
        {
            var driver = new Driver{
                Name=request.Name,
                Phone=request.Phone,
                DriverCode=string.Empty,
                Status = DriverStatus.Active
            };
            
            var created = _repo.Add(driver);
            return CreatedAtAction(nameof(GetByDriverCode), new { driverCode = created.DriverCode }, created);
        }

    [HttpPut("updateStatus")]
    public IActionResult UpdateStatus(UpdateDriverStatusDTO updateDTO)
    { 
        var updated = _repo.UpdateStatus(updateDTO);        
        return NoContent();
    }
    }
}
