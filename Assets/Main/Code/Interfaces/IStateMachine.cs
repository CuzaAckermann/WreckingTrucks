using System;

public interface IStateMachine<S> where S : IState
{
    public event Action<S> StateChanged;

    public S CurrentState { get; }
}
