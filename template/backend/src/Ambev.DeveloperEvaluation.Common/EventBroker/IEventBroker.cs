namespace Ambev.DeveloperEvaluation.Common.EventBroker;

public interface IEventBroker
{
    void Subscribe<EventMessage>(Action<EventMessage> handler) where EventMessage : class;
    void Publish<EventMessage>(EventMessage message) where EventMessage : class;
}