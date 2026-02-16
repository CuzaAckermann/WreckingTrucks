using System;

public abstract class SwitchedApplicationState : ISwitchedApplicationState
{
    public event Action<bool> Toggled;

    public bool IsActive { get; private set; }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;

        Toggled?.Invoke(IsActive);
    }
}