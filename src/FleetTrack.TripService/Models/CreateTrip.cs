namespace FleetTrack.TripService.Models
{
    public class CreateTrip
    { 
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
    }
}
