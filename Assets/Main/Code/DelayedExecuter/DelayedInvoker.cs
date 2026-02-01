using System;
using System.Collections.Generic;

public class DelayedInvoker
{
    private readonly EventBus _eventBus;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly List<ICommandCreator> _commandCreators;

    public DelayedInvoker(EventBus eventBus, StopwatchCreator stopwatchCreator)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _commandCreators = new List<ICommandCreator>();

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<CreatedSignal<ICommandCreator>>(SubscribeToCommandCreator);
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<ICommandCreator>>(SubscribeToCommandCreator);
    }

    private void SubscribeToCommandCreator(CreatedSignal<ICommandCreator> commandCreatorCreatedSignal)
    {
        ICommandCreator commandCreator = commandCreatorCreatedSignal.Creatable;

        commandCreator.DestroyedIDestroyable += UnsubscribeFromCommandCreator;

        commandCreator.CommandCreated += StartDelayedCommand;

        if (_commandCreators.Contains(commandCreator))
        {
            Logger.Log(commandCreator.GetType());

            return;
        }

        _commandCreators.Add(commandCreator);
    }

    private void UnsubscribeFromCommandCreator(IDestroyable destroyable)
    {
        if (destroyable is not ICommandCreator commandCreator)
        {
            throw new InvalidCastException($"{nameof(destroyable)} is not {nameof(commandCreator)}");
        }

        commandCreator.DestroyedIDestroyable -= UnsubscribeFromCommandCreator;

        commandCreator.CommandCreated -= StartDelayedCommand;
    }

    private void StartDelayedCommand(Command command)
    {
        new DelayedCommand(_stopwatchCreator.Create(), command).Start();
    }
}