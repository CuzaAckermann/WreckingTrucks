using System;

public class StopwatchCreator : Factory
{
    public StopwatchCreator(FactorySettings factorySettings) : base(factorySettings)
    {

    }

    public override Type GetCreatableType()
    {
        return typeof(Stopwatch);
    }

    protected override IDestroyable CreateElement()
    {
        return new Stopwatch();
    }
}