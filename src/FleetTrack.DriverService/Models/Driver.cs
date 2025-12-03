using FleetTrack.DriverService.Enums;
namespace FleetTrack.DriverService.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public required string DriverCode { get; set; }

        public required string Name { get; set; }
        public required string Phone { get; set; }

        public required DriverStatus Status { get; set; } = DriverStatus.Active; //Active, Busy, Offline
         
    }
}