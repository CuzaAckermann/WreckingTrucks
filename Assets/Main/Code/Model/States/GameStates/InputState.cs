using System;

public abstract class InputState
{
    public event Action Entered;
    public event Action Exited;

    public virtual void Enter()
    {
        Entered?.Invoke();
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        Exited?.Invoke();
    }
}