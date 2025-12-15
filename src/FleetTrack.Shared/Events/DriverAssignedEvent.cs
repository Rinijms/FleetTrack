namespace FleetTrack.Shared.Events
{
    public class DriverAssignedEvent : BaseEvent
    {
        public string TripCode { get; set; } = string.Empty;
        public string DriverCode { get; set; } = string.Empty;

        public DriverAssignedEvent()
        {
            EventName = "DriverAssigned";
        }
    }
}