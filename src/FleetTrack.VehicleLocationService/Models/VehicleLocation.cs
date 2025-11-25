namespace FleetTrack.VehicleLocationService.Models;

public class VehicleLocation
{
    public int Id { get; set; }
    public string VehicleId{ get; set; } =default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime TimeStampUtc{ get; set; }
}