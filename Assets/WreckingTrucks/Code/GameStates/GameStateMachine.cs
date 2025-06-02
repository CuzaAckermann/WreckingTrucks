using System;

public class GameStateMachine
{
    private IGameState _currentState;
    private IGameState _previousState;

    public GameStateMachine(IGameState gameState)
    {
        _currentState = gameState ?? throw new ArgumentNullException(nameof(gameState));
        _currentState.Enter();
    }

    public void Update(float deltaTime)
    {
        _currentState?.Update(deltaTime);
    }

    public void SwitchState(IGameState newState)
    {
        _previousState = _currentState;
        _currentState?.Exit();
        _currentState = newState ?? throw new ArgumentNullException(nameof(newState));
        _currentState.Enter();
    }

    public void ReturnToPreviousState()
    {
        if (_previousState == null)
        {
            throw new InvalidOperationException(nameof(ReturnToPreviousState));
        }

        SwitchState(_previousState);
    }
}