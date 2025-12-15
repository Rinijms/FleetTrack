namespace FleetTrack.Shared.Events
{
    public class TripCreatedEvent : BaseEvent
    {
        public string TripCode { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;

        public TripCreatedEvent()
        {
            EventName = "TripCreated";
        }
    }
}