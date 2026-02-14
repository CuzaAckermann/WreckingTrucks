using System;
using System.Collections.Generic;

public abstract class InputState<I> : IState where I : IInput
{
    private readonly I _input;

    private bool _shouldStop;

    public InputState(I input)
    {
        Validator.ValidateNotNull(input);

        _input = input;
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
        List<IInputButton> buttons = GetRequiredButtons(_input);

        for (int current = 0; current < buttons.Count && _shouldStop == false; current++)
        {
            buttons[current].Update();
        }
    }

    public virtual void Exit()
    {
        Exited?.Invoke();
    }

    public void Stop()
    {
        _shouldStop = true;
    }

    protected abstract List<IInputButton> GetRequiredButtons(I input);
}