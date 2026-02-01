using System;

public class ModelSelector
{
    private readonly EventBus _eventBus;
    private readonly SphereCastPresenterDetector _presenterDetector;
    private readonly PlayingInput _playingInput;

    public ModelSelector(EventBus eventBus,
                         SphereCastPresenterDetector presenterDetector,
                         PlayingInput playingInput)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _presenterDetector = presenterDetector ? presenterDetector : throw new ArgumentNullException(nameof(presenterDetector));
        _playingInput = playingInput ?? throw new ArgumentNullException(nameof(playingInput));

        SubscribeToPlayingInput();
    }

    private void SubscribeToPlayingInput()
    {
        _playingInput.InteractPressed += SendSelectModelSignal;
    }

    private void UnsubscribeFromPlayingInput()
    {
        _playingInput.InteractPressed -= SendSelectModelSignal;
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