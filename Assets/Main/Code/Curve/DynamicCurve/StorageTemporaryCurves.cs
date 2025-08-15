using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageTemporaryCurves
{
    private readonly Dictionary<int, ModelBezierCurve> _temporaryCurves;
    private readonly BezierCurveSettings _curveSettings;
    private readonly BezierNode _endPoint;

    public StorageTemporaryCurves(BezierCurveSettings bezierCurveSettings,
                                  BezierNode endPoint)
    {
        _curveSettings = bezierCurveSettings ? bezierCurveSettings : throw new ArgumentNullException(nameof(bezierCurveSettings));
        _endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
        _temporaryCurves = new Dictionary<int, ModelBezierCurve>();
    }

    public void CalculateCurves(IReadOnlyList<Model> models)
    {
        Logger.Log(models.Count);

        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        _temporaryCurves.Clear();

        for (int i = 0; i < models.Count; i++)
        {
            Model model = models[i];

            _temporaryCurves[i] = CreateModelBezierCurve(model.Position, model.Forward);
        }

        //Logger.Log(_temporaryCurves.Count);
    }

    public void CalculateCurves(Field field)
    {
        if (field == null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        _temporaryCurves.Clear();

        Vector3 currentPosition = field.Position;

        for (int i = 1; i <= field.AmountColumns; i++)
        {
            _temporaryCurves[i] = CreateModelBezierCurve(currentPosition, field.Forward);
            currentPosition += field.Right * field.IntervalBetweenColumns * i;
        }
    }

    public bool TryGetTemporaryCurve(int index, out ModelBezierCurve curve)
    {
        return _temporaryCurves.TryGetValue(index, out curve);
    }

    private ModelBezierCurve CreateModelBezierCurve(Vector3 startPos,
                                                    Vector3 startForward)
    {
        ModelBezierCurve modelBezierCurve = new ModelBezierCurve(_curveSettings.SegmentsPerSegment,
                                                                 _curveSettings.IsLoop);
        modelBezierCurve.AddNodeAtPosition(startPos, -startForward);
        modelBezierCurve.AddNodeAtPosition(_endPoint.Point.position, _endPoint.TangentIn.position);
        modelBezierCurve.CalculateCurve();

        return modelBezierCurve;
    }
}