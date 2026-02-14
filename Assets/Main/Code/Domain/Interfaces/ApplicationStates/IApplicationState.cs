using System;

public interface IApplicationState : INotifier, IState
{
    public event Action Triggered;
}
