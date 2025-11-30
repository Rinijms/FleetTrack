namespace FleetTrack.DriverService.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public required string DriverCode { get; set; }

        public required string Name { get; set; }
        public required string Phone { get; set; }

        public required string Status { get; set; } = "Active"; //Active, Busy, Offline
         
    }
}