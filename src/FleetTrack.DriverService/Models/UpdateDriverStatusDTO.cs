using FleetTrack.DriverService.Enums;
namespace FleetTrack.DriverService.Models
{
    public class UpdateDriverStatusDTO
    {
        public required string DriverCode { get; set; } 

        public required DriverStatus Status { get; set; }
         
    }
}