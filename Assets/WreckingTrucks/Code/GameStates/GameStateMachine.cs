using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private Stack<GameState> _states = new Stack<GameState>();

    public GameState CurrentState => _states.Count > 0 ? _states.Peek() : null;

    public GameStateMachine(GameState initialState)
    {
        if (initialState == null)
        {
            throw new ArgumentNullException(nameof(initialState));
        }

        PushState(initialState);
    }

    public void Update(float deltaTime)
    {
        CurrentState?.Update(deltaTime);
    }

    public void PushState(GameState newState)
    {
        if (newState == null)
        {
            throw new ArgumentNullException(nameof(newState));
        }

        CurrentState?.Exit();
        _states.Push(newState);
        newState.Enter();
    }

    public void PopState()
    {
        if (_states.Count == 0)
        {
            throw new InvalidOperationException("State stack is empty!");
        }

        GameState oldState = _states.Pop();
        oldState.Exit();

        CurrentState?.Enter();
    }

    public void ReplaceState(GameState newState)
    {
        if (newState == null)
        {
            throw new ArgumentNullException(nameof(newState));
        }

        if (_states.Count == 0)
        {
            throw new InvalidOperationException("State stack is empty!");
        }

        GameState oldState = _states.Pop();
        oldState.Exit();

        _states.Push(newState);
        newState.Enter();
    }

    public void ClearStates()
    {
        foreach (var state in _states)
        {
            state?.Exit();
        }
    }
}