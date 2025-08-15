using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadRenderer : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;
    [SerializeField] private float _roadWidth = 1f;
    [SerializeField] private float _minPointDistance = 0.5f;

    [Header("Line Settings")]
    [SerializeField] private float _widthOfDashes = 0.1f;
    [SerializeField] private Material _lineMaterial;

    [Header("Road Segment")]
    [SerializeField, Range(0f, 1f)] private float _startPercent = 0f;
    [SerializeField, Range(0f, 1f)] private float _endPercent = 1f;

    private const float HalfRoad = 0.5f;

    private List<Vector3> _originalPoints;
    private List<Vector3> _leftLine;
    private List<Vector3> _rightLine;
    private float _currentHeight;

    private bool _isInitialize;

    public void Initialize(List<Vector3> curvePoints, float height)
    {
        if (_isInitialize)
        {
            return;
        }

        ConfigureLineRenderer(_leftLineRenderer);
        ConfigureLineRenderer(_rightLineRenderer);

        _currentHeight = height;
        _originalPoints = curvePoints ?? throw new ArgumentNullException(nameof(curvePoints));
        CalculateRoad();

        _isInitialize = true;
    }

    public void Draw()
    {
        UpdateLineRenderer(_leftLineRenderer, _leftLine);
        UpdateLineRenderer(_rightLineRenderer, _rightLine);
    }

    public void Hide()
    {
        ClearLineRenderer(_leftLineRenderer);
        ClearLineRenderer(_rightLineRenderer);
    }

    private void CalculateRoad()
    {
        List<Vector3> leftBasePoints = ProcessPoints(_originalPoints, _currentHeight);
        _leftLine = CalculateEdgePoints(leftBasePoints, -HalfRoad);

        List<Vector3> rightBasePoints = ProcessPoints(_originalPoints, _currentHeight);
        _rightLine = CalculateEdgePoints(rightBasePoints, HalfRoad);
    }

    private List<Vector3> ProcessPoints(List<Vector3> points, float height)
    {
        List<Vector3> processed = new List<Vector3>();

        Vector3 prevPoint = new Vector3(points[0].x, height, points[0].z);
        processed.Add(prevPoint);

        for (int i = 1; i < points.Count; i++)
        {
            Vector3 currentPoint = new Vector3(points[i].x, height, points[i].z);

            if (Vector3.Distance(currentPoint, prevPoint) >= _minPointDistance)
            {
                processed.Add(currentPoint);
                prevPoint = currentPoint;
            }
        }

        return processed;
    }

    private List<Vector3> CalculateEdgePoints(List<Vector3> basePoints, float widthMultiplier)
    {
        List<Vector3> edgePoints = new List<Vector3>();

        int startIndex = Mathf.FloorToInt((basePoints.Count - 1) * _startPercent);
        int endIndex = Mathf.CeilToInt((basePoints.Count - 1) * _endPercent);
        startIndex = Mathf.Clamp(startIndex, 0, basePoints.Count - 1);
        endIndex = Mathf.Clamp(endIndex, 0, basePoints.Count - 1);

        for (int i = startIndex; i <= endIndex; i++)
        {
            Vector3 forward = CalculateForwardDirection(basePoints, i);
            Vector3 normal = Vector3.Cross(forward, Vector3.up).normalized;
            edgePoints.Add(basePoints[i] + normal * _roadWidth * widthMultiplier);
        }

        return edgePoints;
    }

    private Vector3 CalculateForwardDirection(List<Vector3> points, int index)
    {
        if (index == 0)
        {
            return (points[index + 1] - points[index]).normalized;
        }

        if (index == points.Count - 1)
        {
            return (points[index] - points[index - 1]).normalized;
        }

        Vector3 prevDir = (points[index] - points[index - 1]).normalized;
        Vector3 nextDir = (points[index + 1] - points[index]).normalized;

        return (prevDir + nextDir).normalized;
    }

    private void UpdateLineRenderer(LineRenderer lineRenderer, List<Vector3> points)
    {
        if (lineRenderer == null)
        {
            return;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private void ClearLineRenderer(LineRenderer lineRenderer)
    {
        if (lineRenderer == null)
        {
            return;
        }

        lineRenderer.positionCount = 0;
    }
    private void ConfigureLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = _lineMaterial;

        lineRenderer.startWidth = _widthOfDashes;
        lineRenderer.endWidth = _widthOfDashes;
    }
}