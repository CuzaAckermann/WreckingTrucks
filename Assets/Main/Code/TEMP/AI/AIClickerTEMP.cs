using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class AIClickerTEMP
{
    //private readonly float _startDelay;
    //private readonly float _minFrequency;
    //private readonly float _maxFrequency;

    //private Stopwatch _stopwatch;
    //private GameWorldTEMP _gameWorld;

    //private bool _canSelectNextTruck;

    //public AIClickerTEMP(float startDelay, float minFrequency, float maxFrequency)
    //{
    //    if (startDelay <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(startDelay));
    //    }

    //    if (minFrequency <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(minFrequency));
    //    }

    //    if (maxFrequency <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(maxFrequency));
    //    }

    //    if (minFrequency >= maxFrequency)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(minFrequency));
    //    }

    //    _startDelay = startDelay;
    //    _minFrequency = minFrequency;
    //    _maxFrequency = maxFrequency;
    //    _canSelectNextTruck = false;
    //}

    //public void Prepare(GameWorldTEMP gameWorld)
    //{
    //    _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
    //    _stopwatch = new Stopwatch(Random.Range(_minFrequency, _maxFrequency));
    //    _canSelectNextTruck = true;
    //}

    //public void Start()
    //{
    //    _stopwatch.SetNotificationInterval(_startDelay);
    //    _stopwatch.IntervalPassed += OnIntervalPassed;
    //    _stopwatch.Start();
    //}

    //public void Update(float deltaTime)
    //{
    //    _stopwatch.Tick(deltaTime);
    //}

    //public void Stop()
    //{
    //    _stopwatch.Stop();
    //    _stopwatch.IntervalPassed -= OnIntervalPassed;
    //}

    //private void BecomeReadyToChoose(Model model)
    //{
    //    model.Destroyed -= BecomeReadyToChoose;
    //    _canSelectNextTruck = true;
    //    OnIntervalPassed();
    //}

    //private void OnIntervalPassed()
    //{
    //    if (_canSelectNextTruck)
    //    {
    //        if (TryGetTruck(out Truck selectedTruck))
    //        {
    //            ReleaseTruck(selectedTruck);
    //        }
    //        else
    //        {
    //            int randomIndex = Random.Range(0, _gameWorld.TruckField.AmountColumn);
    //            _gameWorld.TruckField.TryGetFirstElement(randomIndex, out Model model);

    //            if (model is Truck truck)
    //            {
    //                ReleaseTruck(truck);
    //            }
    //        }

    //        _stopwatch.Continue();
    //    }
    //    else
    //    {
    //        _stopwatch.Stop();
    //    }
    //}

    //private void ReleaseTruck(Truck selectedTruck)
    //{
    //    selectedTruck.Destroyed += BecomeReadyToChoose;
    //    _gameWorld.AddTruckOnRoad(selectedTruck);

    //    _stopwatch.SetNotificationInterval(Random.Range(_minFrequency, _maxFrequency));
    //    _canSelectNextTruck = false;
    //}

    //private bool TryGetTruck(out Truck selectedTruck)
    //{
    //    selectedTruck = null;
    //    Dictionary<Type, int> amountElementsOfTypes = CalculateAmountElementOfTypes();

    //    var sortedByValueDesc = amountElementsOfTypes.OrderByDescending(pair => pair.Value);

    //    foreach (var targetAmountElementsOfTypes in sortedByValueDesc)
    //    {
    //        for (int i = 0; i < _gameWorld.TruckField.AmountColumn; i++)
    //        {
    //            if (_gameWorld.TruckField.TryGetFirstElement(i, out Model model))
    //            {
    //                if (model is Truck truck)
    //                {
    //                    if (truck.DestroyableType == targetAmountElementsOfTypes.Key)
    //                    {
    //                        selectedTruck = truck;
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    return false;
    //}

    //private Dictionary<Type, int> CalculateAmountElementOfTypes()
    //{
    //    Dictionary<Type, int> amountElementsOfTypes = new Dictionary<Type, int>();

    //    for (int i = 0; i < _gameWorld.BlockField.AmountColumn; i++)
    //    {
    //        if (_gameWorld.BlockField.TryGetFirstElement(i, out Model model))
    //        {
    //            if (amountElementsOfTypes.ContainsKey(model.GetType()) == false)
    //            {
    //                amountElementsOfTypes.Add(model.GetType(), 0);
    //            }

    //            amountElementsOfTypes[model.GetType()]++;
    //        }
    //    }

    //    return amountElementsOfTypes;
    //}
}