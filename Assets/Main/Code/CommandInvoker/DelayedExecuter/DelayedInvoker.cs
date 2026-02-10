using System;
using System.Collections.Generic;

public class DelayedInvoker
{
    private readonly EventBus _eventBus;
    private readonly IDelayedCommandBuilder _delayedCommandBuilder;
    private readonly List<ICommandCreator> _commandCreators;

    public DelayedInvoker(EventBus eventBus, IDelayedCommandBuilder delayedCommandBuilder)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _delayedCommandBuilder = delayedCommandBuilder ?? throw new ArgumentNullException(nameof(delayedCommandBuilder));
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

        commandCreator.Destroyed += UnsubscribeFromCommandCreator;

        commandCreator.CommandCreated += AddDelayedCommand;

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

        commandCreator.Destroyed -= UnsubscribeFromCommandCreator;

        commandCreator.CommandCreated -= AddDelayedCommand;
    }

    private void AddDelayedCommand(Command command)
    {
        _delayedCommandBuilder.Add(command);
    }
}