using System;

public class StopwatchCreator
{
    private readonly TickEngine _tickEngine;

    public StopwatchCreator(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public Stopwatch Create()
    {
        return new Stopwatch(_tickEngine);
    }
}