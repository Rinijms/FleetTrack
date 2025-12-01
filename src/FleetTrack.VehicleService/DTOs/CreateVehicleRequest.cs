namespace FleetTrack.VehicleService.DTOs;

    public class CreateVehicleRequest
    {
        public required string RegistrationNumber { get; set; } = string.Empty;
        public required string Type { get; set; } = string.Empty;
    }
