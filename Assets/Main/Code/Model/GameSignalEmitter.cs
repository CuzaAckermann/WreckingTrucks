using System;

public class GameSignalEmitter
{
    private readonly EventBus _eventBus;

    public GameSignalEmitter(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public void Clear()
    {
        _eventBus.Invoke(new ClearedSignal<GameSignalEmitter>());
    }

    public void Start()
    {
        _eventBus.Invoke(new EnabledSignal<GameSignalEmitter>());
    }

    public void Stop()
    {
        _eventBus.Invoke(new DisabledSignal<GameSignalEmitter>());
    }
}