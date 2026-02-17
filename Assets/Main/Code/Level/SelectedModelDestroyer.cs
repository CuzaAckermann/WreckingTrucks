public class SelectedModelDestroyer<M> where M : Model
{
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly EventBus _eventBus;

    public SelectedModelDestroyer(ApplicationStateStorage applicationStateStorage, EventBus eventBus)
    {
        Validator.ValidateNotNull(applicationStateStorage, eventBus);

        _applicationStateStorage = applicationStateStorage;
        _eventBus = eventBus;

        _applicationStateStorage.FinishApplicationState.Triggered += Clear;

        _eventBus.Subscribe<SelectedSignal<Model>>(DestroySelected);
    }

    private void Clear()
    {
        _applicationStateStorage.FinishApplicationState.Triggered -= Clear;

        _eventBus.Unsubscribe<SelectedSignal<Model>>(DestroySelected);
    }

    private void DestroySelected(SelectedSignal<Model> selectedSignal)
    {
        if (selectedSignal.Selectable is not M model)
        {
            return;
        }

        model.Destroy();
    }
}