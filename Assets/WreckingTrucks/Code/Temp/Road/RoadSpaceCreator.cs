using UnityEngine;

public class RoadSpaceCreator : MonoBehaviour, IRoadSpaceCreator
{
    [Header("Truck Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 10;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    [Header("Rotator Settings")]
    [SerializeField, Min(1)] private int _capacityRotatables = 5; 
    [SerializeField, Min(1)] private int _rotationSpeed = 20;
    [SerializeField, Min(0.01f)] private int _minAngle = 1;

    [Header("Path Creator Settings")]
    [SerializeField] private PathCreator _pathCreator;

    public RoadSpace CreateRoadSpace()
    {
        Road road = new Road(_pathCreator.CreatePath());
        Mover mover = new Mover(road,
                                _capacityMovables,
                                _movementSpeed,
                                _minSqrDistanceToTargetPosition);

        Rotater rotator = new Rotater(road, _rotationSpeed, _minAngle, _capacityRotatables);

        RoadSpace roadSpace = new RoadSpace(road, mover, rotator);

        return roadSpace;
    }
}