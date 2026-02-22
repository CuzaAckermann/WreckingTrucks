using System;
using System.Collections.Generic;

public class TickEngineRegulator : IApplicationAbility
{
    private readonly StateStackMachine<InputState> _stateMachine;
    private readonly TickEngine _tickEngine;
    private readonly List<Type> _pauseConditions;

    public TickEngineRegulator(StateStackMachine<InputState> stateMachine,
                               TickEngine tickEngine,
                               List<Type> pauseConditions)
    {
        Validator.ValidateNotNull(stateMachine, tickEngine, pauseConditions);

        _stateMachine = stateMachine;
        _tickEngine = tickEngine;
        _pauseConditions = pauseConditions;
    }

    public  void Start()
    {
        _stateMachine.StateChanged += OnInputStateChanged;

        if (_stateMachine.CurrentState == null)
        {
            return;
        }

        OnInputStateChanged(_stateMachine.CurrentState);
    }

    public void Finish()
    {
        _stateMachine.StateChanged -= OnInputStateChanged;
    }

    private void OnInputStateChanged(InputState inputState)
    {
        if (NeedPause(inputState))
        {
            _tickEngine.Pause();
        }
        else
        {
            _tickEngine.Continue();
        }
    }

    private bool NeedPause(InputState inputState)
    {
        bool needPause = false;

        for (int currentCondition = 0; currentCondition < _pauseConditions.Count; currentCondition++)
        {
            if (inputState.GetType() == _pauseConditions[currentCondition])
            {
                needPause = true;

                break;
            }
        }

        return needPause;
    }
}