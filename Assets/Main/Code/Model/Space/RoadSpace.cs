using System;
using System.Collections.Generic;

public class RoadSpace
{
    private readonly Road _road;
    private readonly Mover _truckMover;
    private readonly Rotator _truckRotater;
    private readonly ModelFinalizer _truckFinilizer;

    public RoadSpace(Road road, Mover truckMover, Rotator rotater, ModelFinalizer truckFinalizer)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _truckRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = truckFinalizer ?? throw new ArgumentNullException(nameof(truckFinalizer));
    }

    public void Clear()
    {
        IReadOnlyList<Model> models = _road.GetTrucks();

        _road.Clear();
        _truckMover.Clear();
        _truckRotater.Clear();

        _truckFinilizer.FinishModels(models);
    }

    public void AddTruck(Truck truck)
    {
        _road.AddTruck(truck);
    }

    public void Enable()
    {
        _road.TruckReachedEnd += OnTruckReached;

        _truckMover.Enable();
        _truckRotater.Enable();
    }

    public void Disable()
    {
        _truckMover.Disable();
        _truckRotater.Disable();

        _road.TruckReachedEnd -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishModel(truck);
    }
}