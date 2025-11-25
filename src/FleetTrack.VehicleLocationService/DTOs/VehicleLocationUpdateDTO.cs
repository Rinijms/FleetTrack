namespace FleetTrack.VehicleLocationService.DTOs;

public class VehicleLocationUpdateDTO
{
    public string VehicleId{ get; set; } =default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}