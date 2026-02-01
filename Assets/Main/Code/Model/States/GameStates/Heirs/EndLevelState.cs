using System;

public class EndLevelState : InputState
{
    private readonly EndLevelProcess _endLevelProcess;
    private readonly EventBus _eventBus;

    private Dispencer _dispencer;

    public EndLevelState(EventBus eventBus, EndLevelProcess endLevelProcess)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _endLevelProcess = endLevelProcess ?? throw new ArgumentNullException(nameof(endLevelProcess));

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<CreatedSignal<Dispencer>>(SetDispencer);
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

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencer = createdDispencerSignal.Creatable;
    }
}