using System;
using System.Collections.Generic;

public abstract class InputState<I> : IState where I : IInput
{
    private readonly I _input;
    private readonly List<IInputButton> _inputButtons;

    private bool _shouldStop;

    public InputState(I input)
    {
        Validator.ValidateNotNull(input);

        _input = input;

        _inputButtons = GetRequiredButtons(_input);
    }

    public event Action Entered;
    public event Action Exited;

    public virtual void Enter()
    {
        _shouldStop = false;

        Entered?.Invoke();
    }

    public virtual void Update()
    {
        for (int current = 0; current < _inputButtons.Count && _shouldStop == false; current++)
        {
            _inputButtons[current].Update();
        }
    }

    public virtual void Exit()
    {
        _shouldStop = true;

        Exited?.Invoke();
    }

    protected abstract List<IInputButton> GetRequiredButtons(I input);
}