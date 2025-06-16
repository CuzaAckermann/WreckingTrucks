using System;

public class RoadSpace
{
    private Road _road;
    private Mover _truckMover;
    private Rotater _truckRotater;
    private ModelFinalizer _truckFinilizer;

    public RoadSpace(Road road, Mover truckMover, Rotater rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _truckRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = new ModelFinalizer();
    }

    public void Clear()
    {
        _road.Clear();
        _truckMover.Clear();
        _truckRotater.Clear();
    }

    public void AddTruck(Truck truck)
    {
        _road.AddTruck(truck);
    }

    public void Start()
    {
        _road.TruckFinishedDriving += OnTruckReached;
        _truckMover.Enable();
        _truckRotater.Enable();
    }

    public void Update(float deltaTime)
    {
        _truckMover.Tick(deltaTime);
        _truckRotater.Tick(deltaTime);
    }

    public void Stop()
    {
        _truckMover.Disable();
        _truckRotater.Disable();
        _road.TruckFinishedDriving -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishModel(truck);
    }
}