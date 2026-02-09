using System;

public class EventBusLogger : EventBus
{
    private readonly EventBus _eventBus;

    public EventBusLogger(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public void SubscribeLog<T>(object sender, Action<T> callback, Priority priority = Priority.Medium) where T : EventBusSignal
    {
        Logger.Log(sender.GetType());

        _eventBus.Subscribe(callback, priority);
    }

    public void UnsubscribeLog<T>(object sender, Action<T> callback) where T : EventBusSignal
    {
        Logger.Log(sender.GetType());

        _eventBus.Unsubscribe(callback);
    }
}