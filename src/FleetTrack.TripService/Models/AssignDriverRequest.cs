namespace FleetTrack.TripService.Models
{
    public class AssignDriverRequest
    {
        public string TripCode { get; set;} =string.Empty;
        public string DriverCode { get; set; } =string.Empty;
    }
}