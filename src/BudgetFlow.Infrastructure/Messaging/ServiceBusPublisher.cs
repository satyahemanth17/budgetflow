using System.Text.Json;

namespace BudgetFlow.Infrastructure.Messaging;

public class ServiceBusPublisher
{
    public Task PublishAsync<T>(string topic, T message)
    {
        var payload = JsonSerializer.Serialize(message);
        Console.WriteLine($"[ServiceBus] Topic={topic} Payload={payload}");
        return Task.CompletedTask;
    }
}
