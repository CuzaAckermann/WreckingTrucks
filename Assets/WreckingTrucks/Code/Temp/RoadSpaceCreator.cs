using UnityEngine;

public class RoadSpaceCreator : MonoBehaviour, IRoadSpaceCreator
{
    [Header("Truck Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    [Header("Bullet Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovablesForBulletMover = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeedForBulletMover = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPositionForBulletMover = 0.001f;

    [Header("Rotator Settings")]
    [SerializeField, Min(1)] private int _capacityRotatables = 5; 
    [SerializeField, Min(1)] private int _rotationSpeed = 20;
    [SerializeField, Min(0.01f)] private int _minAngle = 1;

    [Header("Path Creator Settings")]
    [SerializeField] private PathCreator _pathCreator;

    private BulletSimulation _bulletSimulation;

    public RoadSpace CreateRoadSpace()
    {
        Road road = new Road(_pathCreator.CreatePath());
        Mover mover = new Mover(road,
                                _capacityMovables,
                                _movementSpeed,
                                _minSqrDistanceToTargetPosition);

        _bulletSimulation = new BulletSimulation();

        Mover bulletMover = new Mover(_bulletSimulation,
                                      _capacityMovablesForBulletMover,
                                      _movementSpeedForBulletMover,
                                      _minSqrDistanceToTargetPositionForBulletMover);
        Rotater rotator = new Rotater(road, _rotationSpeed, _minAngle, _capacityRotatables);

        RoadSpace roadSpace = new RoadSpace(road, mover, bulletMover, rotator);
        _bulletSimulation.AddRoadSpace(roadSpace);

        return roadSpace;
    }
}