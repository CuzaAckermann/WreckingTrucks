using System;

public class PausedState : InputState
{
    private readonly PauseInputHandler _inputHandler;
    private readonly TickEngine _tickEngine;

    public PausedState(PauseInputHandler inputHandler, TickEngine tickEngine)
    {
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public event Action PauseFinished;

    public override void Enter()
    {
        base.Enter();

        _tickEngine.Pause();
        _inputHandler.PausePressed += OnPauseFinished;
    }

    public override void Update()
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _inputHandler.PausePressed -= OnPauseFinished;
        _tickEngine.Continue();

        base.Exit();
    }

    private void OnPauseFinished()
    {
        PauseFinished?.Invoke();
    }
}