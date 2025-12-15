using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace FleetTrack.Shared.Events
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event) where T : class
        {
            var json = JsonSerializer.Serialize(@event);
            _logger.LogInformation("📢 InMemory Event Published: {EventType} → {Payload}",
                typeof(T).Name, json);

            return Task.CompletedTask;
        }
    }
}