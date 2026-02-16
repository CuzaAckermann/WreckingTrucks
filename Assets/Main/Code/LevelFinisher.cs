public class LevelFinisher
{
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;
    private readonly EndLevelProcess _endLevelProcess;

    private Dispencer _dispencer;

    public LevelFinisher(ApplicationStateStorage applicationStateStorage,
                         EventBus eventBus,
                         EndLevelProcess endLevelProcess)
    {
        Validator.ValidateNotNull(applicationStateStorage, eventBus, endLevelProcess);

        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;
        _endLevelProcess = endLevelProcess;

        _applicationStateStorage.FinishApplicationState.Triggered += Clear;

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
        _applicationStateStorage.FinishApplicationState.Triggered -= Clear;

        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencer = createdDispencerSignal.Creatable;
    }
}