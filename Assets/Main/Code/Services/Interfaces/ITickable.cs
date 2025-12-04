using System;

public interface ITickable
{
    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Tick(float deltaTime);
}