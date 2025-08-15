using System;
using System.Collections.Generic;

public class PlaneSpace : IModelDestroyNotifier
{
    private readonly Road _road;
    private readonly Mover _planeMover;
    private readonly Rotator _planeRotater;

    public PlaneSpace(Road road, Mover truckMover, Rotator rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _planeMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _planeRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
    }

    public event Action<Model> ModelDestroyRequested;

    public event Action<IModel> InterfaceModelDestroyRequested;

    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;

    public void Clear()
    {
        //InterfaceModelsDestroyRequested?.Invoke(_road.GetTrucks());
        _road.Clear();
        _planeMover.Clear();
        _planeRotater.Clear();
    }

    public void AddTruck(Truck truck)
    {
        //_road.AddTruck(truck);
    }

    //public void Enable()
    //{
    //    _road.PathFollowerReachedEnd += OnTruckReached;

    //    _planeMover.Enable();
    //    _planeRotater.Enable();
    //}

    //public void Disable()
    //{
    //    _planeMover.Disable();
    //    _planeRotater.Disable();

    //    _road.PathFollowerReachedEnd -= OnTruckReached;
    //}

    //private void OnTruckReached(IPathFollower pathFollower)
    //{
    //    InterfaceModelDestroyRequested?.Invoke(pathFollower);
    //}
}