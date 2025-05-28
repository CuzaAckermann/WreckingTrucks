using UnityEngine;

public class MoverEngine : MonoBehaviour
{
    [Header("Settings Models")]

    [Header("Settings BlockMover")]
    [SerializeField, Min(1)] private int _capacityListWithBlocks = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeedForBlocks = 5;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPositionForBlocks = 0.001f;

    private Mover<Block> _blocksMover;

    [Header("Settings TrucksMover")]
    [SerializeField, Min(1)] private int _capacityListWithTrucks = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeedForTrucks = 5;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPositionForTrucks = 0.001f;

    private Mover<Truck> _trucksMover;
    private Path _path;

    [Header("Settings Presenters")]

    [Header("Path Presenter")]
    [SerializeField] private PathPresenter _pathPresenter;

    public Mover<Block> BlocksMover => _blocksMover;

    public Mover<Truck> TrucksMover => _trucksMover;

    public void Initialize()
    {
        PrepareBlocksMover();
        PrepareTrucksMover();
    }

    private void PrepareBlocksMover()
    {
        _blocksMover = new Mover<Block>(_capacityListWithBlocks, _movementSpeedForBlocks, _minSqrDistanceToTargetPositionForBlocks);
    }

    private void PrepareTrucksMover()
    {
        _pathPresenter.Initialize();
        _path = new Path(_pathPresenter.Positions);
        _trucksMover = new Mover<Truck>(_capacityListWithTrucks, _movementSpeedForTrucks, _minSqrDistanceToTargetPositionForTrucks);
    }
}