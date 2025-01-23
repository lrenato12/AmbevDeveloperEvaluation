using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.EventBroker;

public class Subscriber<EventMessage> where EventMessage : class
{
    protected readonly IEventBroker _eventBroker;

    public Subscriber(IEventBroker eventBroker)
    {
        _eventBroker = eventBroker;
        eventBroker.Subscribe<EventMessage>(HandleMessage);
    }

    private static void HandleMessage(EventMessage systemEvent)
    {
        string eventMessage = JsonSerializer.Serialize(systemEvent);
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[EVENT] {date} Message received: {eventMessage}");
        Console.ForegroundColor = ConsoleColor.Black;
    }
}