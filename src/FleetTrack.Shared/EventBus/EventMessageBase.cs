namespace FleetTrack.Shared.EventBus
{
    public abstract class EventMessageBase
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; set; } = DateTime.UtcNow;
        public string EventType => GetType().Name;
    }
}