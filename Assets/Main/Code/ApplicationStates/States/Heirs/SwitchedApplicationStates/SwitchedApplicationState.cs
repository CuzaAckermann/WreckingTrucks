using System;

public abstract class SwitchedApplicationState : ISwitchedApplicationState
{
    public event Action<bool> StateChanged;
    public event Action Triggered;

    public bool IsActive { get; private set; }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;

        StateChanged?.Invoke(IsActive);
    }

    public void Enter()
    {

    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}