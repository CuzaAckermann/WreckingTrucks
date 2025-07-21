using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private readonly Stack<GameState> _states = new Stack<GameState>();

    public void ClearStates()
    {
        foreach (GameState state in _states)
        {
            state?.Exit();
        }

        _states.Clear();
    }

    public void PushState(GameState newState)
    {
        if (newState == null)
        {
            throw new ArgumentNullException(nameof(newState));
        }

        if (_states.Count > 0)
        {
            _states.Peek()?.Exit();
        }

        _states.Push(newState);
        newState.Enter();
    }

    public void Update(float deltaTime)
    {
        if (_states.Count > 0)
        {
            _states.Peek()?.Update(deltaTime);
        }
    }

    public void PopState()
    {
        if (_states.Count == 0)
        {
            throw new InvalidOperationException("State stack is empty!");
        }

        GameState oldState = _states.Pop();
        oldState.Exit();

        if (_states.Count > 0)
        {
            _states.Peek()?.Enter();
        }
    }
}