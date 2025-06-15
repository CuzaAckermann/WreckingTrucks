using System;

public class RoadSpace
{
    private Road _road;
    private Mover _truckMover;
    private Rotater _rotater;
    private ModelFinalizer _truckFinilizer;

    public RoadSpace(Road road, Mover truckMover, Rotater rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _rotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = new ModelFinalizer();
    }

    public void Clear()
    {
        _road.Clear();
        _truckMover.Clear();
        _rotater.Clear();
    }

    public void AddTruck(Truck truck)
    {
        _road.AddTruck(truck);
    }

    public void Start()
    {
        _road.TruckFinishedDriving += OnTruckReached;
        _truckMover.Enable();
        _rotater.Start();
    }

    public void Update(float deltaTime)
    {
        _truckMover.Tick(deltaTime);
        _rotater.Tick(deltaTime);
    }

    public void Exit()
    {
        _truckMover.Disable();
        _rotater.Stop();
        _road.TruckFinishedDriving -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishModel(truck);
    }
}