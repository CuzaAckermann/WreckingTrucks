using System.Collections.Generic;

public interface ITimeFlowInput
{
    public IReadOnlyList<ValueButton> TimeButtons { get; }

    public ValueButton TimeSlowDownButton { get; }

    public ValueButton TimeSpeedUpButton { get; }
}