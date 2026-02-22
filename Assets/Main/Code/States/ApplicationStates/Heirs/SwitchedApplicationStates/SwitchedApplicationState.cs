using System;

public abstract class SwitchedApplicationState : ApplicationState
{
    public event Action<bool> Toggled;

    public bool IsActive { get; private set; }

    public void SetIsActive(bool isActive)
    {
        if (isActive == IsActive)
        {
            return;
        }

        IsActive = isActive;

        Toggled?.Invoke(IsActive);
    }
}