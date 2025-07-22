using System;

public class PausedState : GameState
{
    private readonly PauseInputHandler _inputHandler;

    public PausedState(PauseInputHandler inputHandler)
    {
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
    }

    public event Action PauseFinished;

    public override void Enter()
    {
        base.Enter();

        _inputHandler.PausePressed += OnPauseFinished;
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _inputHandler.PausePressed -= OnPauseFinished;

        base.Exit();
    }

    private void OnPauseFinished()
    {
        PauseFinished?.Invoke();
    }
}