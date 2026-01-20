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

    private readonly EventBus _eventBus;

    private GameWorld _gameWorld;

    private BlockField _blockField;
    private TruckField _truckField;

    public ComputerPlayer(Stopwatch stopwatch,
                          TypesCalculator typesCalculator,
                          float startDelay,
                          float minFrequency,
                          float maxFrequency,
                          EventBus eventBus)
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

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetBlockField);
        _eventBus.Subscribe<CreatedSignal<TruckField>>(SetTruckField);
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

        if (TrySelectBestTruck(out Truck truck) || TrySelectRandomTruck(out truck))
        {
            ReleaseTruck(truck);
        }

        _stopwatch.Continue();
    }

    private bool TrySelectBestTruck(out Truck truck)
    {
        Dictionary<ColorType, int> amountElementsOfTypes = _typesCalculator.Calculate(_blockField.GetFirstModels());

        var sortedByValueDesc = amountElementsOfTypes.OrderByDescending(pair => pair.Value);

        truck = null;

        foreach (var targetType in sortedByValueDesc)
        {
            for (int i = 0; i < _truckField.AmountColumns; i++)
            {
                if (_truckField.TryGetFirstModel(0, i, out Model model) == false)
                {
                    continue;
                }

                if (model is Truck selectedTruck == false)
                {
                    continue;
                }

                if (selectedTruck.DestroyableColor != targetType.Key)
                {
                    continue;
                }

                truck = selectedTruck;

                return true;
            }
        }

        return false;
    }

    private bool TrySelectRandomTruck(out Truck truck)
    {
        truck = null;

        int randomIndex = Random.Range(0, _truckField.AmountColumns);
        _truckField.TryGetFirstModel(0, randomIndex, out Model model);

        if (model is Truck randomTruck)
        {
            truck = randomTruck;

            return true;
        }

        return false;
    }

    private void ReleaseTruck(Truck selectedTruck)
    {
        _eventBus.Invoke(new SelectedSignal(selectedTruck));

        //_gameWorld.ReleaseTruck(selectedTruck);

        _stopwatch.SetNotificationInterval(Random.Range(_minFrequency, _maxFrequency));
    }

    private void SetBlockField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _blockField = blockFieldCreatedSignal.Creatable;
    }

    private void SetTruckField(CreatedSignal<TruckField> createdTruckFieldSignal)
    {
        _truckField = createdTruckFieldSignal.Creatable;
    }
}