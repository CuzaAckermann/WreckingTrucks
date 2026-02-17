using System;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    private readonly BezierCurve _mainPath;
    private readonly TemporaryCurvesStorage _storageTemporaryCurves;

    private readonly Dictionary<Model, int> _truckToCurrentPoint;

    public Road(BezierCurve mainPath, BezierCurveSettings settings)
    {
        _mainPath = mainPath ? mainPath : throw new ArgumentNullException(nameof(mainPath));

        if (_mainPath.TryGetFirstNode(out BezierNode node) == false)
        {
            throw new InvalidOperationException($"{nameof(BezierNode)} was not found");
        }

        _storageTemporaryCurves = new TemporaryCurvesStorage(settings, node);
        _truckToCurrentPoint = new Dictionary<Model, int>();
    }

    public void Prepare(Field truckField)
    {
        _storageTemporaryCurves.CalculateCurves(truckField);
    }

    public Vector3 GetFirstPoint()
    {
        return _mainPath.CurvePoints[0];
    }

    public bool TryGetNextPoint(int currentNumberOfPoint, out Vector3 nextPoint)
    {
        if (currentNumberOfPoint < _mainPath.CurvePoints.Count - 1)
        {
            nextPoint = _mainPath.CurvePoints[currentNumberOfPoint + 1];
            return true;
        }

        nextPoint = Vector3.zero;
        return false;
    }
}