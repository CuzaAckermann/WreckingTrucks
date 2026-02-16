using System;
using System.Collections.Generic;

public class StateStackMachine<S> : IStateMachine<S> where S : IState
{
    private readonly Stack<S> _states = new Stack<S>();

    public event Action<S> StateChanged;

    public S CurrentState => _states.Peek();

    public void ClearStates()
    {
        foreach (S state in _states)
        {
            state?.Exit();
        }

        _states.Clear();
    }

    public void PushState(S newState)
    {
        Validator.ValidateNotNull(newState);

        if (_states.Count > 0)
        {
            _states.Peek()?.Exit();

            //Logger.Log(_states.Peek().GetType().Name + " is exited");

            if (_states.Contains(newState))
            {
                do
                {
                    S state = _states.Pop();

                    //Logger.Log(state.GetType().Name + " exit chain");
                }
                while (_states.Contains(newState));
            }
        }

        _states.Push(newState);

        //Logger.Log(newState.GetType());

        UpdateState();
    }

    public void Update()
    {
        if (_states.Count > 0)
        {
            _states.Peek()?.Update();
        }
    }

    public S PopState()
    {
        Validator.ValidateNotEmpty(_states);

        S oldState = _states.Pop();
        oldState.Exit();

        if (_states.Count > 0)
        {
            UpdateState();

            //Logger.Log(_states.Peek().GetType().Name + " is exited");
        }

        return oldState;
    }

    private void UpdateState()
    {
        S newState = _states.Peek();

        newState.Enter();

        StateChanged?.Invoke(newState);
    }

    private void ExitLast()
    {
        S oldState = _states.Pop();
        oldState.Exit();
    }
}