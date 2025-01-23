using System.Collections.Concurrent;

namespace Ambev.DeveloperEvaluation.Common.EventBroker;

public class EventBroker : IEventBroker
{
    private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<TMessage>(Action<TMessage> handler) where TMessage : class
    {
        var messageType = typeof(TMessage);
        if (!_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType] = new List<Delegate>();
        }
        _subscribers[messageType].Add(handler);
    }

    public void Publish<TMessage>(TMessage message) where TMessage : class
    {
        var messageType = typeof(TMessage);
        if (_subscribers.TryGetValue(messageType, out var handlers))
        {
            foreach (var handler in handlers)
            {
                ((Action<TMessage>)handler)(message);
            }
        }
    }
}