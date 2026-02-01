using System;

public class LevelSelectionState : InputState
{
    private readonly TickEngine _tickEngine;

    public LevelSelectionState(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public override void Enter()
    {
        base.Enter();

        _tickEngine.Pause();
    }

    public override void Exit()
    {
        _tickEngine.Continue();

        base.Exit();
    }
}