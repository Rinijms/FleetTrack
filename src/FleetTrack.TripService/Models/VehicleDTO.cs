namespace FleetTrack.TripService.Models
{
    public class VehicleDTO
    { 
        public string VehicleCode { get; set; } =string.Empty;
        public string RegistrationNumber { get; set; } =string.Empty;
        public string Type { get; set; } =string.Empty;
        public int Status { get; set; } = 1; //Active=1, Busy, Offline        
        
    }
}