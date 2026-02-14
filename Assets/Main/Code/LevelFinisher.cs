public class LevelFinisher
{
    private readonly EventBus _eventBus;
    private readonly EndLevelProcess _endLevelProcess;

    private Dispencer _dispencer;

    public LevelFinisher(EventBus eventBus, EndLevelProcess endLevelProcess)
    {
        Validator.ValidateNotNull(eventBus, endLevelProcess);

        _eventBus = eventBus;
        _endLevelProcess = endLevelProcess;

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Clear);

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

    private void Clear(ClearedSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        _dispencer = createdDispencerSignal.Creatable;
    }
}