public class SelectedModelDestroyer<M> where M : Model
{
    private readonly FinishApplicationState _finishApplicationState;
    private readonly EventBus _eventBus;

    public SelectedModelDestroyer(FinishApplicationState finishApplicationState, EventBus eventBus)
    {
        Validator.ValidateNotNull(finishApplicationState, eventBus);

        _finishApplicationState = finishApplicationState;
        _eventBus = eventBus;

        _finishApplicationState.Triggered += Clear;

        _eventBus.Subscribe<SelectedSignal<Model>>(DestroySelected);
    }

    private void Clear()
    {
        _finishApplicationState.Triggered -= Clear;

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