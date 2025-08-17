using System;
using UnityEngine;

[Serializable]
public class RoadSpaceSettings
{
    [SerializeField] private MoverSettings _moverSettings;
    [SerializeField] private RotatorSettings _rotatorSettings;
    [SerializeField] private BezierCurve _pathForTrucks;

    public MoverSettings MoverSettings => _moverSettings;

    public RotatorSettings RotatorSettings => _rotatorSettings;

    public BezierCurve PathForTrucks => _pathForTrucks;

    public void SetPath(BezierCurve path)
    {
        _pathForTrucks = path;
    }
}