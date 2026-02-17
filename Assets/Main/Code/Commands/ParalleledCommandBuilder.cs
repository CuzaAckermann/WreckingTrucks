using System;

public class ParalleledCommandBuilder : IDelayedCommandBuilder
{
    private readonly StopwatchCreator _stopwatchCreator;

    public ParalleledCommandBuilder(StopwatchCreator stopwatchCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
    }

    public void Add(Command command)
    {
        new DelayedCommand(_stopwatchCreator.Create(), command).Start();
    }
}