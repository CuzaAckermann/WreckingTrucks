using System;

public class FillingState
{
    private readonly FillingStrategy _fillingStrategy;

    private Action _handlerForFillingFinished;

    private bool _isSubscribed;

    public FillingState(FillingStrategy fillingStrategy)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
    }

    public void Enter(Action handlerForFillingFinished)
    {
        if (_isSubscribed == false)
        {
            _handlerForFillingFinished = handlerForFillingFinished ?? throw new ArgumentNullException(nameof(handlerForFillingFinished));

            _fillingStrategy.FillingFinished += _handlerForFillingFinished;
            _fillingStrategy.Enable();

            _isSubscribed = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _fillingStrategy.Disable();
            _fillingStrategy.FillingFinished -= _handlerForFillingFinished;

            _isSubscribed = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }
}