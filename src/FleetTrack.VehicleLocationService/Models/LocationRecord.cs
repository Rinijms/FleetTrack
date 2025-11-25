namespace FleetTrack.VehicleLocationService.Models;

public class LocationRecord
{
    public int Id { get; set; }
    public string VehicleId{ get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}