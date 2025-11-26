namespace FleetTrack.TripService.Models
{
    public class Trip
    {
        public Guid TripId { get; set; } = Guid.NewGuid();
        public string TripCode { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
    }
}