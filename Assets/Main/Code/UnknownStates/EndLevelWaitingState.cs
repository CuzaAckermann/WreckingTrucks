using System;

public class EndLevelWaitingState
{
    private readonly Field _blockField;
    private readonly CartrigeBoxField _cartrigeBoxField;

    private readonly Action _successfulCompletionHandler;
    private readonly Action _failedCompletionHandler;

    private bool _isSubscribed;

    public EndLevelWaitingState(Field blockField,
                                CartrigeBoxField cartrigeBoxField,
                                Action successfulCompletionHandler,
                                Action failedCompletionHandler)
    {
        _blockField = blockField ?? throw new ArgumentNullException(nameof(blockField));
        _cartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));

        _successfulCompletionHandler = successfulCompletionHandler ?? throw new ArgumentNullException(nameof(successfulCompletionHandler));
        _failedCompletionHandler = failedCompletionHandler ?? throw new ArgumentNullException(nameof(failedCompletionHandler));

        _isSubscribed = false;
    }

    public void Enter()
    {
        if (_isSubscribed == false)
        {
            _blockField.Devastated += _successfulCompletionHandler;
            _cartrigeBoxField.Devastated += _failedCompletionHandler;

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
            _blockField.Devastated -= _successfulCompletionHandler;
            _cartrigeBoxField.Devastated -= _failedCompletionHandler;

            _isSubscribed = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }
}