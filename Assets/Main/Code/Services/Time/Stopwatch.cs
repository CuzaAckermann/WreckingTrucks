using System;

public class Stopwatch : ITickable
{
    private readonly TickEngine _tickEngine;

    private float _currentTime;
    private float _notificationIntervalInSeconds;
    private bool _isRunned;

    public Stopwatch(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public event Action IntervalPassed;

    public bool IsRunning => _tickEngine.Contains(this);

    public void SetNotificationInterval(float notificationIntervalInSeconds)
    {
        if (notificationIntervalInSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(notificationIntervalInSeconds)} must be positive");
        }

        _notificationIntervalInSeconds = notificationIntervalInSeconds;
    }

    public void Reset()
    {
        _currentTime = 0;
        Stop();
    }

    public void Start()
    {
        _currentTime = 0;
        Continue();
    }

    public void Continue()
    {
        if (_isRunned == false)
        {
            _isRunned = true;
            _tickEngine.AddTickable(this);
        }
    }

    public void Stop()
    {
        if (_isRunned)
        {
            _isRunned = false;
            _tickEngine.RemoveTickable(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (deltaTime < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(deltaTime)} cannot be negative.");
        }

        _currentTime += deltaTime;

        if (_currentTime >= _notificationIntervalInSeconds)
        {
            _currentTime -= _notificationIntervalInSeconds;
            IntervalPassed?.Invoke();
        }
    }
}