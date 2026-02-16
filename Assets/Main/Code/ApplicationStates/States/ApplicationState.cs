using System;

public abstract class ApplicationState : IApplicationState
{
    public event Action Triggered;

    public void Trigger()
    {
        Triggered?.Invoke();
    }
}