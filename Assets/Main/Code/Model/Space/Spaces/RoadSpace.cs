using System;
using System.Collections.Generic;

public class RoadSpace : IModelDestroyNotifier
{
    private readonly Road _road;
    private readonly Mover _truckMover;
    private readonly Rotator _truckRotater;

    public RoadSpace(Road road, Mover truckMover, Rotator rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _truckRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
    }



    public event Action<Model> ModelDestroyRequested;

    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<IModel> InterfaceModelDestroyRequested;

    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;



    public void Clear()
    {
        ModelsDestroyRequested?.Invoke(_road.MovableTrucks);

        _road.Clear();
        _truckMover.Clear();
        _truckRotater.Clear();
    }

    public void AddTruck(Truck truck, int indexOfColumn)
    {
        _road.AddTruck(truck, indexOfColumn);
    }

    public void Prepare(Field truckField)
    {
        _road.Prepare(truckField);
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
        InterfaceModelDestroyRequested?.Invoke(truck);
        ModelDestroyRequested?.Invoke(truck);
    }
}