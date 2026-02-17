using System;

public interface ISwitchedApplicationState
{
    public event Action<bool> Toggled;

    public bool IsActive { get; }
}