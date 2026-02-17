using System;
using UnityEngine;

[Serializable]
public class RoadSpaceSettings
{
    [SerializeField] private BezierCurve _pathForTrucks;

    public BezierCurve PathForTrucks => _pathForTrucks;

    public void SetPath(BezierCurve path)
    {
        _pathForTrucks = path;
    }
}