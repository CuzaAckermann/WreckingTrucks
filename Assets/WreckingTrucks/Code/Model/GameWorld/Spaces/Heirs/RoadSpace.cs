using System;

public class RoadSpace
{
    private readonly Road _road;
    private readonly Mover _truckMover;
    private readonly Rotator _truckRotater;
    private readonly ModelFinalizer _truckFinilizer;

    private readonly TickEngine _tickEngine;
    private readonly TickEngine _truckTickEngine;

    public RoadSpace(Road road, Mover truckMover, Rotator rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _truckRotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = new ModelFinalizer();

        _tickEngine = new TickEngine();
        _truckTickEngine = new TickEngine();
    }

    public void Clear()
    {
        _truckMover.Clear();
        _truckRotater.Clear();
        _road.Clear();
        _tickEngine.Clear();
        _truckTickEngine.Clear();
    }

    public void Prepare()
    {
        _tickEngine.AddTickable(_truckMover);
        _tickEngine.AddTickable(_truckRotater);
    }

    public void AddTruck(Truck truck)
    {
        _truckTickEngine.AddTickable(truck);
        _road.AddTruck(truck);
    }

    public void Start()
    {
        _road.TruckFinishedDriving += OnTruckReached;
        _truckMover.Enable();
        _truckRotater.Enable();

        _tickEngine.Continue();
        _truckTickEngine.Continue();
    }

    public void Update(float deltaTime)
    {
        _tickEngine.Tick(deltaTime);
        _truckTickEngine.Tick(deltaTime);
    }

    public void Stop()
    {
        _tickEngine.Pause();
        _truckTickEngine.Pause();

        _truckMover.Disable();
        _truckRotater.Disable();
        _road.TruckFinishedDriving -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishModel(truck);
    }
}