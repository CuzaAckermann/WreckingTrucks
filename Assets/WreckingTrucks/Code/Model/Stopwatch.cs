using System;

public class Stopwatch : ITickable, IResetable
{
    private bool _isActivated;
    private float _currentTime;
    private float _notificationIntervalInSeconds;

    public Stopwatch(float notificationIntervalInSeconds)
    {
        if (notificationIntervalInSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(notificationIntervalInSeconds)} must be greater than zero.");
        }

        _notificationIntervalInSeconds = notificationIntervalInSeconds;
    }

    public event Action IntervalPassed;

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
        _isActivated = false;
    }

    public void Start()
    {
        _currentTime = 0;
        _isActivated = true;
    }

    public void Continue()
    {
        _isActivated = true;
    }

    public void Tick(float deltaTime)
    {
        if (_isActivated)
        {
            if (deltaTime < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(deltaTime)} must be greater than zero.");
            }

            _currentTime += deltaTime;

            if (_currentTime >= _notificationIntervalInSeconds)
            {
                _currentTime -= _notificationIntervalInSeconds;
                IntervalPassed?.Invoke();
            }
        }
    }

    public void Stop()
    {
        _isActivated = false;
    }
}