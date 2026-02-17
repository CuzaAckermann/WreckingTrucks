using System;

public class StopwatchCreator : Factory<Stopwatch>, ITickableCreator
{
    public StopwatchCreator(FactorySettings factorySettings) : base(factorySettings)
    {
        InitPool(FactorySettings.InitialPoolSize,
                 FactorySettings.MaxPoolCapacity);
    }

    public event Action<ITickable> TickableCreated;

    protected override Stopwatch CreateElement()
    {
        return new Stopwatch();
    }

    public override Stopwatch Create()
    {
        Stopwatch stopwatch = base.Create();

        TickableCreated?.Invoke(stopwatch);

        return stopwatch;
    }
}