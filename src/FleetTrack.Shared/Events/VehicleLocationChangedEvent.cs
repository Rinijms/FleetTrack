namespace FleetTrack.Common.Events
{
    public class VehicleLocationChangedEvent
    {
        public string VehicleCode { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Optional metadata
        public string Source { get; set; } = "VehicleLocationService";
    }
}