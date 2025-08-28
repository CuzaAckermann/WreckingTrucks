using System;
using UnityEngine;

[Serializable]
public class PlaneSpaceSettings
{
    [SerializeField] private Transform _planeSlotPosition;
    [SerializeField] private MoverSettings _moverSettings;
    [SerializeField] private RotatorSettings _rotatorSettings;
    [SerializeField] private BezierCurve _pathForPlane;
    [SerializeField] private int _amountOfUses;

    public Transform PlaneSlotPosition => _planeSlotPosition;

    public MoverSettings MoverSettings => _moverSettings;

    public RotatorSettings RotatorSettings => _rotatorSettings;

    public BezierCurve PathForPlane => _pathForPlane;

    public int AmountOfUses => _amountOfUses;

    public void SetSettings(Transform planeSlotPosition,
                                     BezierCurve pathForPlane)
    {
        _planeSlotPosition = planeSlotPosition;
        _pathForPlane = pathForPlane;
    }
}