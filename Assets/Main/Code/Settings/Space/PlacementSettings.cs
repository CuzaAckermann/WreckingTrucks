using UnityEngine;

public class PlacementSettings : MonoBehaviour
{
    [Header("Field Positions")]
    [SerializeField] private Transform _blockField;
    [SerializeField] private Transform _truckField;
    [SerializeField] private Transform _cartrigeBoxField;

    public Transform BlockField => _blockField;

    public Transform TruckField => _truckField;

    public Transform CartrigeBoxField => _cartrigeBoxField;
}