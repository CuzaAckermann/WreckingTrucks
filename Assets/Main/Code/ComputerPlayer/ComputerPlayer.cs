using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class ComputerPlayer
{
    private readonly Stopwatch _stopwatch;
    private readonly TypesCalculator _typesCalculator;
    private readonly float _startDelay;
    private readonly float _minFrequency;
    private readonly float _maxFrequency;

    private GameWorld _gameWorld;

    public ComputerPlayer(Stopwatch stopwatch,
                          TypesCalculator typesCalculator,
                          float startDelay,
                          float minFrequency,
                          float maxFrequency)
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

        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _typesCalculator = typesCalculator ?? throw new ArgumentNullException(nameof(typesCalculator));

        _startDelay = startDelay;
        _minFrequency = minFrequency;
        _maxFrequency = maxFrequency;
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
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

        if (TryReleaseBestTruck() == false)
        {
            SelectRandomTruck();
        }

        _stopwatch.Continue();
    }

    private bool TryReleaseBestTruck()
    {
        Dictionary<ColorType, int> amountElementsOfTypes = _typesCalculator.Calculate(_gameWorld.BlockField.GetFirstModels());

        var sortedByValueDesc = amountElementsOfTypes.OrderByDescending(pair => pair.Value);

        foreach (var targetType in sortedByValueDesc)
        {
            for (int i = 0; i < _gameWorld.TruckField.AmountColumns; i++)
            {
                if (_gameWorld.TruckField.TryGetFirstModel(0, i, out Model model) == false)
                {
                    continue;
                }

                if (model is Truck truck == false)
                {
                    continue;
                }

                if (truck.DestroyableColor != targetType.Key)
                {
                    continue;
                }

                ReleaseTruck(truck);

                return true;
            }
        }

        return false;
    }

    private void SelectRandomTruck()
    {
        int randomIndex = Random.Range(0, _gameWorld.TruckField.AmountColumns);
        _gameWorld.TruckField.TryGetFirstModel(0, randomIndex, out Model model);

        if (model is Truck truck)
        {
            ReleaseTruck(truck);
        }
    }

    private void ReleaseTruck(Truck selectedTruck)
    {
        _gameWorld.ReleaseTruck(selectedTruck);

        _stopwatch.SetNotificationInterval(Random.Range(_minFrequency, _maxFrequency));
    }
}