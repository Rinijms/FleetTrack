using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FleetTrack.Shared.EventBus;

public class RabbitMqProducer : IEventBusProducer
{
    private readonly string _connectionString;
    private readonly string _exchange;

    public RabbitMqProducer(string connectionString, string exchange = "fleettrack.events")
    {
        _connectionString = connectionString;
        _exchange = exchange;
    }

    public Task PublishAsync<T>(T @event) where T : EventMessageBase
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_connectionString)
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(_exchange, type: ExchangeType.Fanout, durable: true);

        var payload = JsonSerializer.SerializeToUtf8Bytes(@event);

        channel.BasicPublish(
            exchange: _exchange,
            routingKey: "",
            basicProperties: null,
            body: payload
        );

        return Task.CompletedTask;
    }
}