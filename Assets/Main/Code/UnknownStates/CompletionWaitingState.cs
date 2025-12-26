using System;

public class CompletionWaitingState
{
    private readonly ICompletionNotifier _notifier;

    private Action _handler;

    private bool _isSubscribed;

    public CompletionWaitingState(ICompletionNotifier notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public void Enter(Action handler)
    {
        if (_isSubscribed == false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));

            _notifier.Completed += _handler;

            _isSubscribed = true;
        }
        else
        {
            //Logger.Log($"Already subscribed to {GetType()}");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _notifier.Completed -= _handler;

            _isSubscribed = false;
        }
        else
        {
            //Logger.Log($"Already unsubscribed from {GetType()}");
        }
    }
}