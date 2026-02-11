using System;

public class Stopwatch : ITickable
{
    private float _notificationIntervalInSeconds;

    public Stopwatch()
    {
        Time = new Amount(0);
    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public event Action IntervalPassed;

    public event Action<IDestroyable> Destroyed;

    public Amount Time { get; private set; }

    public bool IsRunned { get; private set; }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

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
        Time.Change(0);

        Stop();
    }

    public void Start()
    {
        Time.Change(0);

        Continue();
    }

    public void Continue()
    {
        if (IsRunned)
        {
            return;
        }

        IsRunned = true;

        Activated?.Invoke(this);
    }

    public void Stop()
    {
        if (IsRunned == false)
        {
            return;
        }

        IsRunned = false;

        Deactivated?.Invoke(this);
    }

    public void Tick(float deltaTime)
    {
        if (deltaTime < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(deltaTime)} cannot be negative.");
        }

        Time.Increase(deltaTime);

        if (_notificationIntervalInSeconds <= 0)
        {
            return;
        }

        if (Time.Value >= _notificationIntervalInSeconds)
        {
            Time.Decrease(_notificationIntervalInSeconds);

            IntervalPassed?.Invoke();
        }
    }
}