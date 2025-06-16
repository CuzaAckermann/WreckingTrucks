using System;
using System.Collections.Generic;
using UnityEngine;

public class Road : IPositionsModelsChangedNotifier
{
    private readonly Path _path;
    private readonly List<Truck> _trucks;

    public Road(Path path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
        _trucks = new List<Truck>();
    }

    public event Action<List<Model>> TargetPositionsModelsChanged;
    public event Action<Truck> TruckFinishedDriving;

    public void Clear()
    {
        for (int i = 0; i < _trucks.Count; i++)
        {
            UnsubscribeFromTruck(_trucks[i]);
        }

        _trucks.Clear();
    }

    public void AddTruck(Truck truck)
    {
        if (truck == null)
        {
            throw new ArgumentNullException(nameof(truck));
        }

        if (_trucks.Contains(truck))
        {
            throw new InvalidOperationException(nameof(truck));
        }

        _trucks.Add(truck);
        SubscribeToTruck(truck);
        SetNextPosition(truck, _path.GetFirstPosition());
    }

    private void OnCurrentPositionReached(Truck truck)
    {
        if (_path.TryGetNextPosition(truck.Position, out Vector3 nextPosition))
        {
            SetNextPosition(truck, nextPosition);
        }
        else
        {
            UnsubscribeFromTruck(truck);
            TruckFinishedDriving?.Invoke(truck);
        }
    }

    private void SetNextPosition(Truck truck, Vector3 nextPosition)
    {
        truck.SetTargetPosition(nextPosition);
        TargetPositionsModelsChanged?.Invoke(new List<Model> { truck });
    }

    private void SubscribeToTruck(Truck truck)
    {
        truck.CurrentPositionReached += OnCurrentPositionReached;
    }

    private void UnsubscribeFromTruck(Truck truck)
    {
        if (truck == null)
        {
            throw new ArgumentNullException(nameof(truck));
        }

        truck.CurrentPositionReached -= OnCurrentPositionReached;
        _trucks.Remove(truck);
    }
}