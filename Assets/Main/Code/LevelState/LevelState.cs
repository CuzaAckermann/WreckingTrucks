using System;

public class LevelState
{
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;

    private Level _level;

    public LevelState(ApplicationStateStorage applicationStateStorage, EventBus eventBus)
    {
        Validator.ValidateNotNull(applicationStateStorage, eventBus);

        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;

        _eventBus.Subscribe<CreatedSignal<Level>>(SetLevel);

        _applicationStateStorage.StopApplicationState.Triggered += Finish;
    }

    public void Enter()
    {
        _level?.Enable();
    }

    private void Finish()
    {
        _applicationStateStorage.StopApplicationState.Triggered += Finish;

        _eventBus.Unsubscribe<CreatedSignal<Level>>(SetLevel);
    }

    private void SetLevel(CreatedSignal<Level> createdSignal)
    {
        DestroyLevel();

        _level = createdSignal.Creatable;
    }

    private void DestroyLevel()
    {
        _level?.Disable();
        _level?.Clear();

        _level = null;
    }
}