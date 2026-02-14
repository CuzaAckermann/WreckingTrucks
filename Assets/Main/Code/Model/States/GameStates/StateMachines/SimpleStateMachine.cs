using System;

public class SimpleStateMachine<S> : IStateMachine<S> where S : IState
{
    private S _currentApplicationState;

    public event Action<S> StateChanged;

    public S CurrentState => _currentApplicationState;

    public void SwitchState(S nextApplicationState)
    {
        _currentApplicationState?.Exit();

        Validator.ValidateNotNull(nextApplicationState);
        _currentApplicationState = nextApplicationState;
        StateChanged?.Invoke(_currentApplicationState);

        _currentApplicationState.Enter();
    }
}