using System;
using UnityEngine;

public class Movers : MonoBehaviour
{
    [Header("Settings BlockMover")]
    [SerializeField, Min(1)] private int _capacityListWithBlocks = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeedForBlocks = 5;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPositionForBlocks = 0.001f;

    [Header("Settings TrucksMover")]
    [SerializeField, Min(1)] private int _capacityListWithTrucks = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeedForTrucks = 5;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPositionForTrucks = 0.001f;

    private Mover<Block> _blocksMover;
    private Mover<Truck> _trucksMover;
    private ITickEngineUpdaterOnlyAddAndRemove _tickEngineUpdater;

    public Mover<Block> BlocksMover => _blocksMover;

    public Mover<Truck> TrucksMover => _trucksMover;

    public void Initialize(ITickEngineUpdaterOnlyAddAndRemove tickEngineUpdater)
    {
        _tickEngineUpdater = tickEngineUpdater ?? throw new ArgumentNullException(nameof(tickEngineUpdater));

        _blocksMover = new Mover<Block>(_capacityListWithBlocks,
                                        _movementSpeedForBlocks,
                                        _minSqrDistanceToTargetPositionForBlocks);

        _trucksMover = new Mover<Truck>(_capacityListWithTrucks,
                                        _movementSpeedForTrucks,
                                        _minSqrDistanceToTargetPositionForTrucks);
    }

    public void Clear()
    {
        _blocksMover.Clear();
        _trucksMover.Clear();

        RemoveTickables();
    }

    public void Prepare()
    {
        AddTickables();
    }

    private void AddTickables()
    {
        _tickEngineUpdater.Add(_blocksMover);
        _tickEngineUpdater.Add(_trucksMover);
    }

    private void RemoveTickables()
    {
        _tickEngineUpdater.Remove(_blocksMover);
        _tickEngineUpdater.Remove(_trucksMover);
    }
}