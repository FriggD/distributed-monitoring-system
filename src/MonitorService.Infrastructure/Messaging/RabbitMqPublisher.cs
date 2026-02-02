using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using MonitorService.Application.Interfaces;

namespace MonitorService.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _hostname;

    public RabbitMqPublisher(string hostname = "localhost")
    {
        _hostname = hostname;
    }

    private async Task EnsureConnectedAsync()
    {
        if (_channel != null) return;

        var factory = new ConnectionFactory { HostName = _hostname };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync("monitoring-events", ExchangeType.Topic, durable: true);
    }

    public async Task PublishAsync<T>(string routingKey, T message)
    {
        await EnsureConnectedAsync();
        
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel!.BasicPublishAsync(
            exchange: "monitoring-events",
            routingKey: routingKey,
            body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
