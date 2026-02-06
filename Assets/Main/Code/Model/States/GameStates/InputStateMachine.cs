using System;
using System.Collections.Generic;

public class InputStateMachine
{
    private readonly Stack<InputState> _states = new Stack<InputState>();

    public void ClearStates()
    {
        foreach (InputState state in _states)
        {
            state?.Exit();
        }

        _states.Clear();
    }

    public void PushState(InputState newState)
    {
        if (newState == null)
        {
            throw new ArgumentNullException(nameof(newState));
        }

        if (_states.Count > 0)
        {
            _states.Peek()?.Exit();

            //Logger.Log(_states.Peek().GetType().Name + " is exited");

            if (_states.Contains(newState))
            {
                do
                {
                    InputState inputState = _states.Pop();

                    //Logger.Log(inputState.GetType().Name + " exit chain");
                }
                while (_states.Contains(newState));
            }
        }

        _states.Push(newState);

        //Logger.Log(newState.GetType());

        newState.Enter();
    }

    public void Update()
    {
        if (_states.Count > 0)
        {
            _states.Peek()?.Update();
        }
    }

    public InputState PopState()
    {
        if (_states.Count == 0)
        {
            throw new InvalidOperationException("State stack is empty!");
        }

        InputState oldState = _states.Pop();
        oldState.Exit();

        if (_states.Count > 0)
        {
            _states.Peek()?.Enter();

            //Logger.Log(_states.Peek().GetType().Name + " is exited");
        }

        return oldState;
    }
}