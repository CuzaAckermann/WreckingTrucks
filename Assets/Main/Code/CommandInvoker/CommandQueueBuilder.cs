using System;
using System.Collections.Generic;

public class CommandQueueBuilder : IDelayedCommandBuilder
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly List<Command> _waitingCommands;

    public CommandQueueBuilder(StopwatchCreator stopwatchCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
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

        new DelayedCommand(_stopwatchCreator.Create(), command).Start();
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
