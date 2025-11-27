namespace FleetTrack.VehicleService.Models;

public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();         // internal PK
    public string VehicleCode { get; set; } = string.Empty; // external short code, e.g., VH-0001
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;       // e.g., Truck, Van, Car
    public string Status { get; set; } = "Active";        // Active, InService, Maintenance
}