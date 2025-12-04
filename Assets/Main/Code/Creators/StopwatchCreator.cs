using System;

public class StopwatchCreator : Factory<Stopwatch>, ITickableCreator
{
    public StopwatchCreator(FactorySettings factorySettings) : base(factorySettings)
    {
        InitPool(FactorySettings.InitialPoolSize,
                 FactorySettings.MaxPoolCapacity);
    }

    public event Action<ITickable> StopwatchCreated;

    protected override Stopwatch CreateElement()
    {
        return new Stopwatch();
    }

    public override Stopwatch Create()
    {
        Stopwatch stopwatch = base.Create();

        StopwatchCreated?.Invoke(stopwatch);

        return stopwatch;
    }

    //public Stopwatch Create()
    //{
    //    Stopwatch stopwatch = new Stopwatch();

    //    Created?.Invoke(stopwatch);

    //    return stopwatch;
    //}
}