namespace Ambev.DeveloperEvaluation.Common.EventBroker;

public class Publisher
{
    private readonly IEventBroker _eventBroker;

    public Publisher(IEventBroker eventBroker)
    {
        _eventBroker = eventBroker;
    }

    public void PublishMessage<EventMessage>(EventMessage eventMessage) where EventMessage : class
    {
        _eventBroker.Publish(eventMessage);
    }
}