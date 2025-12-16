using System;

public class StopwatchWaitingState
{
    private readonly Stopwatch _stopwatch;

    private Action _handler;

    private bool _isSubscribed;

    public StopwatchWaitingState(Stopwatch stopwatch, float frequency)
    {
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _stopwatch.SetNotificationInterval(frequency);
    }

    public void Enter(Action handler)
    {
        if (_isSubscribed == false)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));

            _stopwatch.IntervalPassed += _handler;
            _isSubscribed = true;

            _stopwatch.Start();
        }
        else
        {
            Logger.Log($"Already subscribed to {GetType()}");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _stopwatch.Stop();

            _stopwatch.IntervalPassed -= _handler;
            _isSubscribed = false;
        }
        else
        {
            Logger.Log($"Already unsubscribed from {GetType()}");
        }
    }
}