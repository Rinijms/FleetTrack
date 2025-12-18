using System.Text;
using System.Text.Json;
using FleetTrack.DriverService.Data;
using FleetTrack.DriverService.Enums;
using FleetTrack.Shared.EventBus;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FleetTrack.DriverService.Messaging;

public class TripCompletedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;

    private IConnection? _connection;
    private IModel? _channel;
 
    private const string QueueName = "Driver.Trip.Completed";    

    public TripCompletedConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _config = config;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_config["RabbitMQ:ConnectionString"]!)
        };
        var exchangeConfig = _config["RabbitMq:Exchange"];

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: exchangeConfig,
            type: ExchangeType.Fanout,
            durable: true);

        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            queue: QueueName,
            exchange: exchangeConfig,
            routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DriverDbContext>();

            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonSerializer.Deserialize<TripCompletedEvent>(json);

                if (evt == null || string.IsNullOrEmpty(evt.DriverCode))
                    return;

                var driver = await db.Drivers
                    .FirstOrDefaultAsync(d => d.DriverCode == evt.DriverCode);

                if (driver == null)
                    return;

                driver.Status = DriverStatus.Active;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DriverService consumer error: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}