using System;
using Random = UnityEngine.Random;

public class AIClicker
{
    private readonly float _startDelay;
    private readonly float _minFrequency;
    private readonly float _maxFrequency;

    private Stopwatch _stopwatch;
    private GameWorld _gameWorld;

    public AIClicker(float startDelay, float minFrequency, float maxFrequency)
    {
        if (startDelay <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startDelay));
        }

        if (minFrequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minFrequency));
        }

        if (maxFrequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxFrequency));
        }

        if (minFrequency >= maxFrequency)
        {
            throw new ArgumentOutOfRangeException(nameof(minFrequency));
        }

        _startDelay = startDelay;
        _minFrequency = minFrequency;
        _maxFrequency = maxFrequency;
        
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
        _stopwatch = new Stopwatch(Random.Range(_minFrequency, _maxFrequency));
    }

    public void Start()
    {
        _stopwatch.SetNotificationInterval(_startDelay);
        _stopwatch.IntervalPassed += OnIntervalPassed;
        _stopwatch.Start();
    }

    public void Update(float deltaTime)
    {
        _stopwatch.Tick(deltaTime);
    }

    public void Stop()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= OnIntervalPassed;
    }

    private void OnIntervalPassed()
    {
        int indexColumn = Random.Range(0, _gameWorld.TruckField.AmountColumn);

        if (_gameWorld.TruckField.TryGetFirstElement(indexColumn, out Model model))
        {
            _gameWorld.AddTruckOnRoad((Truck)model);
        }

        _stopwatch.SetNotificationInterval(Random.Range(_minFrequency, _maxFrequency));
    }
}