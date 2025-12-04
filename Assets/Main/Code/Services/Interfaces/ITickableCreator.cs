using System;

public interface ITickableCreator
{
    public event Action<ITickable> StopwatchCreated;
}