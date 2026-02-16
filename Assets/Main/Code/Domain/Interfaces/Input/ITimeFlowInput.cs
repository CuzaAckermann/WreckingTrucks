using System.Collections.Generic;

public interface ITimeFlowInput
{
    public IReadOnlyList<TimeButton> TimeButtons { get; }

    public TimeButton DecreasedTimeButton { get; }

    public TimeButton IncreasedTimeButton { get; }
}