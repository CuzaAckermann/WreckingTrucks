using System;

public class Stopwatch : ITickable,
                         IDestroyable,
                         IAmountChangedNotifier
{
    private float _notificationIntervalInSeconds;

    public Stopwatch()
    {

    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public event Action IntervalPassed;

    public event Action<IDestroyable> DestroyedIDestroyable;

    public event Action<float> AmountChanged;

    public float CurrentTime { get; private set; }

    public bool IsRunned { get; private set; }

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
        UpdateTime(0);
        Stop();
    }

    public void Start()
    {
        UpdateTime(0);
        Continue();
    }

    public void Continue()
    {
        if (IsRunned == false)
        {
            IsRunned = true;

            Activated?.Invoke(this);
        }
    }

    public void Stop()
    {
        if (IsRunned)
        {
            IsRunned = false;

            Deactivated?.Invoke(this);
        }
    }

    public void Tick(float deltaTime)
    {
        if (deltaTime < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(deltaTime)} cannot be negative.");
        }

        UpdateTime(CurrentTime + deltaTime);

        if (_notificationIntervalInSeconds <= 0)
        {
            return;
        }

        if (CurrentTime >= _notificationIntervalInSeconds)
        {
            UpdateTime(CurrentTime - _notificationIntervalInSeconds);
            IntervalPassed?.Invoke();
        }
    }

    public void Destroy()
    {
        DestroyedIDestroyable?.Invoke(this);
    }

    public int GetMaxAmount()
    {
        throw new NotImplementedException();
    }

    private void UpdateTime(float nextTime)
    {
        CurrentTime = nextTime;
        AmountChanged?.Invoke(CurrentTime);
    }
}