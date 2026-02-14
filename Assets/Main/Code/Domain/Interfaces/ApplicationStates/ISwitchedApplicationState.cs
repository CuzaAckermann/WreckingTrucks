using System;

public interface ISwitchedApplicationState : IApplicationState
{
    public event Action<bool> StateChanged;

    public bool IsActive { get; }
}