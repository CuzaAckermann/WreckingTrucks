using System;

public abstract class ApplicationState : IApplicationState
{
    public event Action Triggered;

    public void Enter()
    {

    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    public void Trigger()
    {
        Triggered?.Invoke();
    }
}