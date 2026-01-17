using System;

public class EndLevelState : GameState
{
    private readonly EndLevelProcess _endLevelProcess;
    private readonly EventBus _eventBus;

    private Dispencer _dispencer;

    public EndLevelState(EventBus eventBus, EndLevelProcess endLevelProcess)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _endLevelProcess = endLevelProcess ?? throw new ArgumentNullException(nameof(endLevelProcess));

        _eventBus.Subscribe<CreatedDispencerSignal>(SetDispencer);
    }

    public override void Enter()
    {
        base.Enter();

        _endLevelProcess.SetDispencer(_dispencer);
        _endLevelProcess.Enable();
    }

    public override void Exit()
    {
        _endLevelProcess.Disable();
        _endLevelProcess.Clear();

        base.Exit();
    }

    private void SetDispencer(CreatedDispencerSignal createdDispencerSignal)
    {
        _dispencer = createdDispencerSignal.Dispencer;
    }
}