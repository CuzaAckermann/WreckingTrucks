using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelBezierCurve
{
    private const int MinAllowedAmountNodes = 2;

    private readonly List<ModelBezierNode> _nodes;
    private readonly List<Vector3> _curvePoints;
    private readonly bool _isClosedLoop = false;

    private int _segmentsPerSegment;

    public ModelBezierCurve(int segmentsPerSegment, bool isClosedLoop)
    {
        if (segmentsPerSegment <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(segmentsPerSegment));
        }

        _segmentsPerSegment = segmentsPerSegment;
        _isClosedLoop = isClosedLoop;
        _nodes = new List<ModelBezierNode>();
        _curvePoints = new List<Vector3>();
    }

    public bool ClosedLoop => _isClosedLoop;

    public int AmountCurvePoints => _curvePoints.Count;

    public float Length { get; private set; }

    public void AddNodeAtPosition(Vector3 position, Vector3 tangentIn)
    {
        _nodes.Add(new ModelBezierNode(position, tangentIn));
    }

    public void AddNode(ModelBezierNode bezierNode)
    {
        if (bezierNode == null)
        {
            throw new ArgumentNullException(nameof(bezierNode));
        }

        _nodes.Add(bezierNode);
    }

    public void RemoveNode(int index)
    {
        if (index < 0 || index >= _nodes.Count)
        {
            return;
        }

        _nodes.RemoveAt(index);
    }

    public Vector3 GetCurvePoint(int index)
    {
        if (index < 0 || index >= _curvePoints.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _curvePoints[index];
    }

    public Vector3 GetPointOnCurve(float normalizedPosition)
    {
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        float totalLength = normalizedPosition * (_curvePoints.Count - 1);
        int pointIndex = Mathf.FloorToInt(totalLength);
        float segmentInterpolation = totalLength - pointIndex;

        if (pointIndex < _curvePoints.Count - 1)
        {
            return Vector3.Lerp(_curvePoints[pointIndex], _curvePoints[pointIndex + 1], segmentInterpolation);
        }

        return _curvePoints[_curvePoints.Count - 1];
    }
    
    public void CalculateCurve()
    {
        _curvePoints.Clear();

        if (_nodes.Count < MinAllowedAmountNodes)
        {
            return;
        }

        int segments = _isClosedLoop ? _nodes.Count : _nodes.Count - 1;

        for (int i = 0; i < segments; i++)
        {
            int currentIndex = i;
            int nextIndex = (i + 1) % _nodes.Count;

            ModelBezierNode currentNode = _nodes[currentIndex];
            ModelBezierNode nextNode = _nodes[nextIndex];

            if (currentNode == null || nextNode == null)
            {
                continue;
            }

            Vector3 point0 = currentNode.Point;
            Vector3 point1 = currentNode.TangentOut;
            Vector3 point2 = nextNode.TangentIn;
            Vector3 point3 = nextNode.Point;

            for (int j = 0; j <= _segmentsPerSegment; j++)
            {
                float step = j / (float)_segmentsPerSegment;
                _curvePoints.Add(CalculateBezierPoint(step, point0, point1, point2, point3));
            }
        }

        Length = GetLength();
    }

    private float GetLength()
    {
        float length = 0f;
        int segments = _isClosedLoop ? _curvePoints.Count : _curvePoints.Count - 1;

        for (int i = 0; i < segments; i++)
        {
            // тут корень
            length += Vector3.Distance(_curvePoints[i], _curvePoints[(i + 1) % _nodes.Count]);
        }

        return length;
    }

    private Vector3 CalculateBezierPoint(float step, Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        float u = 1 - step;
        float tt = step * step;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * step;

        return uuu * point0 +
               3 * uu * step * point1 +
               3 * u * tt * point2 +
               ttt * point3;
    }
}