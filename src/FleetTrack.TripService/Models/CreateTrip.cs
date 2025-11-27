namespace FleetTrack.TripService.Models
{
    public class CreateTrip
    { 
        public string VehicleCode { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
    }
}
