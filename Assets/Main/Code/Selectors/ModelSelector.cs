public class ModelSelector
{
    private readonly EventBus _eventBus;
    private readonly SphereCastPresenterDetector _presenterDetector;
    private readonly IInput _input;
    private readonly PlayingInputState _playingInputState;

    public ModelSelector(EventBus eventBus,
                         SphereCastPresenterDetector presenterDetector,
                         IInput input,
                         PlayingInputState playingInputState)
    {
        Validator.ValidateNotNull(eventBus, presenterDetector, input, playingInputState);

        _eventBus = eventBus;
        _presenterDetector = presenterDetector;
        _input = input;
        _playingInputState = playingInputState;

        SubscribeToPlayingInput();
    }

    private void SubscribeToPlayingInput()
    {
        _input.InteractButton.Pressed += SendSelectModelSignal;
    }

    private void UnsubscribeFromPlayingInput()
    {
        _input.InteractButton.Pressed -= SendSelectModelSignal;
    }

    private void SendSelectModelSignal()
    {
        if (_presenterDetector.TryGetPresenter(out Presenter presenter) == false)
        {
            return;
        }

        _eventBus.Invoke(new SelectedSignal(presenter.Model));
    }
}