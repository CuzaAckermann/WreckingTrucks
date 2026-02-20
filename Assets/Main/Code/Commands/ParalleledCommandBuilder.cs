using System;

public class ParalleledCommandBuilder : IDelayedCommandBuilder
{
    private readonly Production _production;

    public ParalleledCommandBuilder(Production production)
    {
        Validator.ValidateNotNull(production);

        _production = production;
    }

    public void Add(Command command)
    {
        if (_production.TryCreate(out Stopwatch stopwatch) == false)
        {
            throw new InvalidOperationException($"{nameof(Stopwatch)} was not created.");
        }

        new DelayedCommand(stopwatch, command).Start();
    }
}