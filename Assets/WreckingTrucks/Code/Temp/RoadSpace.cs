using System;

public class RoadSpace
{
    private Road _road;
    private Mover _truckMover;
    private Mover _bulletsMover;
    private Rotater _rotater;
    private TruckFinilizer _truckFinilizer;

    public RoadSpace(Road road, Mover truckMover, Mover bulletsMover, Rotater rotater)
    {
        _road = road ?? throw new ArgumentNullException(nameof(road));
        _truckMover = truckMover ?? throw new ArgumentNullException(nameof(truckMover));
        _bulletsMover = bulletsMover ?? throw new ArgumentNullException(nameof(bulletsMover));
        _rotater = rotater ?? throw new ArgumentNullException(nameof(rotater));
        _truckFinilizer = new TruckFinilizer();
    }

    public event Action<Gun> GunReady;

    public void Clear()
    {
        _road.Clear();
        _truckMover.Clear();
        _bulletsMover.Clear();
        _rotater.Clear();
    }

    public void AddTruck(Truck truck)
    {
        _road.AddTruck(truck);
        GunReady?.Invoke(truck.Gun);
    }

    public void Start()
    {
        _road.TruckFinishedDriving += OnTruckReached;
        _rotater.Start();
    }

    public void Update(float deltaTime)
    {
        _truckMover.Tick(deltaTime);
        _bulletsMover.Tick(deltaTime);
        _rotater.Tick(deltaTime);
    }

    public void Exit()
    {
        _rotater.Exit();
        _road.TruckFinishedDriving -= OnTruckReached;
    }

    private void OnTruckReached(Truck truck)
    {
        _truckFinilizer.FinishTruck(truck);
    }
}