using UnityEngine;

public class PlacementSettings : MonoBehaviour
{
    [Header("Field Positions")]
    [SerializeField] private Transform _blockFieldPosition;
    [SerializeField] private Transform _truckFieldPosition;
    [SerializeField] private Transform _cartrigeBoxFieldPosition;

    [Header("Paths")]
    [SerializeField] private BezierCurve _pathForTrucks;
    [SerializeField] private BezierCurve _pathForPlane;

    [Header("PlaneSlot")]
    [SerializeField] private Transform _planeSlotPosition;

    public Transform BlockFieldPosition => _blockFieldPosition;

    public Transform TruckFieldPosition => _truckFieldPosition;

    public Transform CartrigeBoxFieldPosition => _cartrigeBoxFieldPosition;

    public BezierCurve PathForTrucks => _pathForTrucks;

    public BezierCurve PathForPlane => _pathForPlane;

    public Transform PlaneSlotPosition => _planeSlotPosition;
}