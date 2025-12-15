namespace FleetTrack.Common.Events
{
    public class VehicleStatusChangedEvent
    {
        public string VehicleCode { get; set; } = string.Empty;
        public int OldStatus { get; set; }
        public int NewStatus { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Optional metadata
        public string Source { get; set; } = "VehicleService";
    }
}