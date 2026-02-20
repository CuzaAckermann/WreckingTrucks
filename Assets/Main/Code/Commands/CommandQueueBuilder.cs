using System;
using System.Collections.Generic;

public class CommandQueueBuilder : IDelayedCommandBuilder
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly List<Command> _waitingCommands;

    public CommandQueueBuilder(StopwatchCreator stopwatchCreator)
    {
        Validator.ValidateNotNull(stopwatchCreator);

        _stopwatchCreator = stopwatchCreator;
        _waitingCommands = new List<Command>();
    }

    public event Action CommandsCompleted;

    public void Clear()
    {
        for (int currentCommand = _waitingCommands.Count - 1; currentCommand >= 0; currentCommand--)
        {
            UnsubscribeFromCommand(_waitingCommands[currentCommand]);
        }

        _waitingCommands.Clear();
    }

    public void Add(Command command)
    {
        _waitingCommands.Add(command);

        RemoveNulls();

        if (_waitingCommands.Count == 1)
        {
            StartCommand(command);
        }
    }

    private void StartNextCommand(Command completedCommand)
    {
        UnsubscribeFromCommand(completedCommand);

        RemoveNulls();

        if (_waitingCommands.Count == 0)
        {
            CommandsCompleted?.Invoke();

            return;
        }

        StartCommand(_waitingCommands[0]);
    }

    private void StartCommand(Command command)
    {
        SubscribeToCommand(command);

        if (Validator.IsRequiredType(_stopwatchCreator.Create(), out Stopwatch stopwatch) == false)
        {
            throw new InvalidOperationException();
        }

        new DelayedCommand(stopwatch, command).Start();
    }

    private void SubscribeToCommand(Command command)
    {
        command.Canceled += UnsubscribeFromCommand;
        command.Executed += StartNextCommand;
    }

    private void UnsubscribeFromCommand(Command command)
    {
        command.Canceled -= UnsubscribeFromCommand;
        command.Executed -= StartNextCommand;

        _waitingCommands[_waitingCommands.IndexOf(command)] = null;
    }

    private void RemoveNulls()
    {
        for (int currentCommand = _waitingCommands.Count - 1; currentCommand >= 0; currentCommand--)
        {
            if (_waitingCommands[currentCommand] == null)
            {
                _waitingCommands.RemoveAt(currentCommand);
            }
        }
    }
}
