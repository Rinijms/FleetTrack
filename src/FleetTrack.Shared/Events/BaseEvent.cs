namespace FleetTrack.Shared.Events
{
    public abstract class BaseEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public string EventName { get; set; } = string.Empty;
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public string? CorrelationId { get; set; }
    }
}