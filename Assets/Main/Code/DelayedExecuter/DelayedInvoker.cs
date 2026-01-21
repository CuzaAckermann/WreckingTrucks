using System;
using System.Collections.Generic;
using UnityEngine;

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

        _eventBus.Subscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Subscribe<CreatedSignal<ICommandCreator>>(AddCommandCreator);
    }

    private void Clear(ClearedSignal<Game> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<ICommandCreator>>(AddCommandCreator);

        for (int commandCreator = _commandCreators.Count - 1; commandCreator >= 0; commandCreator--)
        {
            _commandCreators[commandCreator].CommandCreated -= AddCommand;
            _commandCreators.RemoveAt(commandCreator);
        }
    }

    private void AddCommandCreator(CreatedSignal<ICommandCreator> commandCreatorCreatedSignal)
    {
        ICommandCreator commandCreator = commandCreatorCreatedSignal.Creatable;

        if (_commandCreators.Contains(commandCreator))
        {
            return;
        }

        _commandCreators.Add(commandCreator);
        commandCreator.CommandCreated += AddCommand;
    }

    private void AddCommand(Command command)
    {

    }
}