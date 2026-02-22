public abstract class WindowHandler<W> : WindowHandlerBase where W : StateWindowBase
{
    private readonly W _stateWindow;

    public WindowHandler(W stateWindow, InputStateStorage storage) : base(storage)
    {
        Validator.ValidateNotNull(stateWindow);

        _stateWindow = stateWindow;
    }

    public override void Start()
    {
        SubscribeToWindow(_stateWindow);
    }

    public override void Finish()
    {
        UnsubscribeFromWindow(_stateWindow);
    }

    protected abstract void SubscribeToWindow(W window);

    protected abstract void UnsubscribeFromWindow(W window);
}