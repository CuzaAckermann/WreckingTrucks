using System;
using System.Collections.Generic;

public class DelayedInvoker : IApplicationAbility
{
    private readonly EventBus _eventBus;
    private readonly IDelayedCommandBuilder _delayedCommandBuilder;
    private readonly List<ICommandCreator> _commandCreators;

    public DelayedInvoker(EventBus eventBus, IDelayedCommandBuilder delayedCommandBuilder)
    {
        Validator.ValidateNotNull(eventBus, delayedCommandBuilder);

        _eventBus = eventBus;
        _delayedCommandBuilder = delayedCommandBuilder;
        _commandCreators = new List<ICommandCreator>();
    }

    public void Start()
    {
        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(SubscribeToCommandCreator);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<CreatedSignal<IDestroyable>>(SubscribeToCommandCreator);
    }

    private void SubscribeToCommandCreator(CreatedSignal<IDestroyable> createdSignal)
    {
        if (Validator.IsRequiredType(createdSignal.Creatable, out ICommandCreator commandCreator) == false)
        {
            return;
        }

        if (_commandCreators.Contains(commandCreator))
        {
            //Logger.Log(commandCreator.GetType());

            return;
        }

        commandCreator.Destroyed += UnsubscribeFromCommandCreator;

        commandCreator.CommandCreated += AddDelayedCommand;

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