namespace FleetTrack.Shared.EventBus;

public class VehicleLocationUpdatedEvent : EventMessageBase
{
    public string VehicleCode { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime ReportedAtUtc { get; set; }
}