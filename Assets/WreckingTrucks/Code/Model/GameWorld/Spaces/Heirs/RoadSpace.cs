using System;

public class RoadSpace
{
    private Road _road;
    private Mover _truckMover;
    private Rotator _truckRotater;
    private ModelFinalizer _truckFinilizer;

    private TickEngine _tickEngine;

    public RoadSpace(Road road, Mover truckMover, Rotator rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _truckRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = new ModelFinalizer();

        _tickEngine = new TickEngine();
    }

    public void Clear()
    {
        _truckMover.Clear();
        _truckRotater.Clear();
        _road.Clear();
    }

    public void Prepare()
    {
        _tickEngine.AddTickable(_truckMover);
        _tickEngine.AddTickable(_truckRotater);
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

        _tickEngine.Continue();
    }

    public void Update(float deltaTime)
    {
        _truckMover.Tick(deltaTime);
        _truckRotater.Tick(deltaTime);
    }

    public void Stop()
    {
        _tickEngine.Pause();

        _truckMover.Disable();
        _truckRotater.Disable();
        _road.TruckFinishedDriving -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishModel(truck);
    }
}