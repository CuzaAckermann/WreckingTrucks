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
            _trucks[i].Destroy();
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
        SetNextCheckPoint(truck, _path.GetFirstCheckPoint());
    }

    private void OnCurrentPositionReached(Truck truck)
    {
        if (_path.TryGetNextCheckPoint(truck.CurrentCheckPoint, out CheckPoint nextCheckPoint))
        {
            SetNextCheckPoint(truck, nextCheckPoint);
        }
        else
        {
            UnsubscribeFromTruck(truck);
            _trucks.Remove(truck);
            TruckFinishedDriving?.Invoke(truck);
        }
    }

    private void SetNextCheckPoint(Truck truck, CheckPoint nextCheckPoint)
    {
        truck.SetCheckPoint(nextCheckPoint);
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
    }
}