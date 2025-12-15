namespace FleetTrack.Shared.EventBus;

public class TripCompletedEvent : EventMessageBase
{
    public string TripCode { get; set; } = string.Empty;
    public string DriverCode { get; set; } =  string.Empty;
    public string VehicleCode { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    
}