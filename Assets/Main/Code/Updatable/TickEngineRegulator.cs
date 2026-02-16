using System;
using System.Collections.Generic;

public class TickEngineRegulator : IAbility
{
    private readonly StateStackMachine<InputState<IInput>> _stateMachine;
    private readonly TickEngine _tickEngine;
    private readonly List<Type> _pauseConditions;

    public TickEngineRegulator(StateStackMachine<InputState<IInput>> stateMachine,
                               TickEngine tickEngine)
    {
        Validator.ValidateNotNull(stateMachine, tickEngine);

        _stateMachine = stateMachine;
        _tickEngine = tickEngine;

        _pauseConditions = new List<Type>()
        {
            typeof(LevelSelectionInputState),
            typeof(PausedInputState)
        };
    }

    public  void Start()
    {
        _stateMachine.StateChanged += OnInputStateChanged;
        OnInputStateChanged(_stateMachine.CurrentState);
    }

    public void Finish()
    {
        _stateMachine.StateChanged -= OnInputStateChanged;
    }

    private void OnInputStateChanged(InputState<IInput> inputState)
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

    private bool NeedPause(InputState<IInput> inputState)
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