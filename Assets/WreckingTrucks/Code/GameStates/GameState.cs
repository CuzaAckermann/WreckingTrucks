using System;

public abstract class GameState : IGameState
{
    private IWindow _window;

    public GameState(IWindow window)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
    }

    public abstract void Update(float deltaTime);

    public virtual void Enter()
    {
        _window.Show();
    }

    public virtual void Exit()
    {
        _window.Hide();
    }
}