public class LevelFinisher
{
    private readonly FinishApplicationState _finishApplicationState;
    private readonly EventBus _eventBus;
    private readonly EndLevelProcess _endLevelProcess;

    private Dispencer _dispencer;

    public LevelFinisher(FinishApplicationState finishApplicationState,
                         EventBus eventBus,
                         EndLevelProcess endLevelProcess)
    {
        Validator.ValidateNotNull(finishApplicationState, eventBus, endLevelProcess);

        _finishApplicationState = finishApplicationState;
        _eventBus = eventBus;
        _endLevelProcess = endLevelProcess;

        _finishApplicationState.Triggered += Clear;

        _eventBus.Subscribe<CreatedSignal<Dispencer>>(SetDispencer);
    }

    public void Enter()
    {
        _endLevelProcess.SetDispencer(_dispencer);
        _endLevelProcess.Enable();
    }

    public void Exit()
    {
        _endLevelProcess.Disable();
        _endLevelProcess.Clear();
    }

    private void Clear()
    {
        _finishApplicationState.Triggered -= Clear;

        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencer = createdDispencerSignal.Creatable;
    }
}