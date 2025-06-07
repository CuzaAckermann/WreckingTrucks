using System;

public abstract class GameState
{
    public event Action Entered;
    public event Action Exited;

    public virtual void Update(float deltaTime) { }

    public virtual void Enter()
    {
        Entered?.Invoke();
    }

    public virtual void Exit()
    {
        Exited?.Invoke();
    }
}