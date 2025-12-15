namespace FleetTrack.Shared.Events
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : class;
    }
}