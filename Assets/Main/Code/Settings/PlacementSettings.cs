using UnityEngine;

public class PlacementSettings : MonoBehaviour
{
    [Header("Field Positions")]
    [SerializeField] private Transform _blockFieldTransform;
    [SerializeField] private Transform _truckFieldTransform;
    [SerializeField] private Transform _cartrigeBoxFieldTransform;

    [Header("Paths")]
    [SerializeField] private BezierCurve _pathForTrucks;
    [SerializeField] private BezierCurve _pathForPlane;

    [Header("PlaneSlot")]
    [SerializeField] private Transform _planeSlotPosition;

    public Transform BlockFieldTransform => _blockFieldTransform;

    public Transform TruckFieldTransform => _truckFieldTransform;

    public Transform CartrigeBoxFieldTransform => _cartrigeBoxFieldTransform;

    public BezierCurve PathForTrucks => _pathForTrucks;

    public BezierCurve PathForPlane => _pathForPlane;

    public Transform PlaneSlotPosition => _planeSlotPosition;
}