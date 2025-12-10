namespace FleetTrack.TripService.Models
{
    public class AssignVehicleRequest
    {
        public string TripCode { get; set; } = string.Empty;
        public string VehicleCode { get; set; } = string.Empty;
    }
}