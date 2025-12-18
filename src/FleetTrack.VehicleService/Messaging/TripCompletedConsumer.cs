using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using FleetTrack.Shared.EventBus;
using FleetTrack.VehicleService.Repositories;
using FleetTrack.VehicleService.Enums;
using FleetTrack.VehicleService.Models;

namespace FleetTrack.VehicleService.Messaging;

public class TripCompletedConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IModel? _channel;
     
    private const string QueueName = "Vehicle.Trip.Completed";

    public TripCompletedConsumer(
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration["RabbitMQ:ConnectionString"]!)
        };
        var exchangeConfig = _configuration["RabbitMq:Exchange"];

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: exchangeConfig,
            type: ExchangeType.Fanout,
            durable: true
        );
 
        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            queue: QueueName,
            exchange: exchangeConfig,
            routingKey: ""
        );

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<TripCompletedEvent>(json);

                if (@event == null) return;

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();

                var vehicle = repo.GetByVehicleCode(@event.VehicleCode);
                if (vehicle == null) return;

                UpdateVehicleStatusDTO updateVehicleStatus= new UpdateVehicleStatusDTO{
                VehicleCode=vehicle.VehicleCode,
                Status = VehicleStatus.Active,
                };

                repo.UpdateStatus(updateVehicleStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing TripCompletedEvent: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: true,
            consumer: consumer
        );

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}