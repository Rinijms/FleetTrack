namespace FleetTrack.VehicleService.DTOs;

    public class CreateVehicleRequest
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
