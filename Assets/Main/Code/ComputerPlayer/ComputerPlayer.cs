using System;
using Random = UnityEngine.Random;

public class ComputerPlayer
{
    private readonly EventBus _eventBus;
    private readonly TruckSelector _truckSelector;

    private readonly Stopwatch _stopwatch;
    private readonly float _startDelay;
    private readonly float _minFrequency;
    private readonly float _maxFrequency;

    public ComputerPlayer(EventBus eventBus,
                          TruckSelector truckSelector,
                          Stopwatch stopwatch,
                          float startDelay,
                          float minFrequency,
                          float maxFrequency)
    {
        if (minFrequency >= maxFrequency)
        {
            throw new ArgumentOutOfRangeException(nameof(minFrequency));
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _truckSelector = truckSelector ?? throw new ArgumentNullException(nameof(truckSelector));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));

        _startDelay = startDelay > 0 ? startDelay : throw new ArgumentOutOfRangeException(nameof(startDelay));
        _minFrequency = minFrequency > 0 ? minFrequency : throw new ArgumentOutOfRangeException(nameof(minFrequency));
        _maxFrequency = maxFrequency > 0 ? maxFrequency : throw new ArgumentOutOfRangeException(nameof(maxFrequency));
    }

    public void Enable()
    {
        _stopwatch.SetNotificationInterval(_startDelay);

        _stopwatch.IntervalPassed += OnIntervalPassed;
        _stopwatch.Start();
    }

    public void Disable()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= OnIntervalPassed;
    }

    private void OnIntervalPassed()
    {
        _stopwatch.Stop();

        if (_truckSelector.TrySelectTruck(out Truck truck))
        {
            ReleaseTruck(truck);
        }

        _stopwatch.Continue();
    }

    private void ReleaseTruck(Truck selectedTruck)
    {
        _eventBus.Invoke(new SelectedSignal(selectedTruck));

        _stopwatch.SetNotificationInterval(Random.Range(_minFrequency, _maxFrequency));
    }
}