using System;
using System.Collections.Generic;

public class Road : IModelPositionObserver
{
    private readonly Path _path;
    private readonly List<Truck> _trucks;

    public Road(Path path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
        _trucks = new List<Truck>();
    }

    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action<Truck> TruckReachedEnd;

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
        SetNextCheckPoint(truck, _path.GetFirstCheckPoint());
    }

    public IReadOnlyList<Truck> GetTrucks()
    {
        return _trucks;
    }

    private void SetNextCheckPoint(Truck truck, CheckPoint nextCheckPoint)
    {
        truck.SetCheckPoint(nextCheckPoint);
        PositionChanged?.Invoke(truck);
    }

    private void SubscribeToTruck(Truck truck)
    {
        truck.TargetCheckPointReached += OnCurrentPositionReached;
        truck.Destroyed += UnsubscribeFromTruck;
    }

    private void UnsubscribeFromTruck(Model destroyedModel)
    {
        destroyedModel.Destroyed -= UnsubscribeFromTruck;

        if (destroyedModel is Truck truck)
        {
            truck.TargetCheckPointReached -= OnCurrentPositionReached;
        }
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
            TruckReachedEnd?.Invoke(truck);
        }
    }
}