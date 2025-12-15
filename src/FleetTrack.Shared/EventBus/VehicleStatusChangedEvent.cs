namespace FleetTrack.Shared.EventBus;

public class VehicleStatusChangedEvent : EventMessageBase
{
    public string VehicleCode { get; set; } = string.Empty;
    public int OldStatus { get; set; }
    public int NewStatus { get; set; }
}