using System;

public class Stopwatch : ITickable
{
    private float _notificationIntervalInSeconds;
    private float _currentTime;
    private bool _isActivated;

    public Stopwatch(float notificationIntervalInSeconds)
    {
        if (notificationIntervalInSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(notificationIntervalInSeconds),
                                                  $"{nameof(notificationIntervalInSeconds)} must be greater than zero.");
        }

        _notificationIntervalInSeconds = notificationIntervalInSeconds;
    }

    public event Action IntervalPassed;

    public void Start()
    {
        _currentTime = 0;
        _isActivated = true;
    }

    public void Tick(float deltaTime)
    {
        if (_isActivated)
        {
            if (deltaTime < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaTime),
                                                      $"{nameof(deltaTime)} must be greater than zero.");
            }

            _currentTime += deltaTime;

            if (_currentTime >= _notificationIntervalInSeconds)
            {
                _currentTime = 0;
                IntervalPassed?.Invoke();
            }
        }
    }

    public void Stop()
    {
        _isActivated = false;
    }
}