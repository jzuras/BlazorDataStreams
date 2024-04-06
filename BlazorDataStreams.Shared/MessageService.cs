namespace BlazorDataStreams.Shared;

public interface IMessageService
{
    Task PublishAsync<TMessage>(TMessage message);
    Task SubscribeAsync<TMessage>(Func<TMessage, Task> handler);
}

public class MessageService : IMessageService
{
    private readonly List<Func<object, Task>> _subscribers = new List<Func<object, Task>>();

    public async Task PublishAsync<TMessage>(TMessage message)
    {
        foreach (var subscriber in _subscribers.OfType<Func<TMessage, Task>>())
        {
            await subscriber(message);
        }
    }

    public async Task SubscribeAsync<TMessage>(Func<TMessage, Task> handler)
    {
        _subscribers.Add(obj => obj is TMessage msg ? handler(msg) : Task.CompletedTask);
    }
}
