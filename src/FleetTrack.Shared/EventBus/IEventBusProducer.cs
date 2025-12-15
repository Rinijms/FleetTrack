namespace FleetTrack.Shared.EventBus
{
    public interface IEventBusProducer
    {
        Task PublishAsync<T>(T @event) where T : EventMessageBase;
    }
}