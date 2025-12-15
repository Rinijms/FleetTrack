namespace FleetTrack.Shared.Events
{
    public class VehicleAssignedEvent : BaseEvent
    {
        public string TripCode { get; set; } = string.Empty;
        public string VehicleCode { get; set; } = string.Empty;

        public VehicleAssignedEvent()
        {
            EventName = "VehicleAssigned";
        }
    }
}
