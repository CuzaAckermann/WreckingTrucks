using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveLineRenderer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Line Settings")]
    [SerializeField] private float _widthOfDashes = 0.1f;
    [SerializeField] private Material _lineMaterial;

    public void Init()
    {
        ConfigureLineRenderer();
    }

    public void Clear()
    {
        _lineRenderer.positionCount = 0;
    }

    public void DrawBorders(ModelBezierCurve bezierCurve)
    {
        if (bezierCurve == null)
        {
            throw new ArgumentNullException(nameof(bezierCurve));
        }

        _lineRenderer.loop = bezierCurve.ClosedLoop;
        List<Vector3> curvePoints = new List<Vector3>();

        for (int i = 0; i < bezierCurve.AmountCurvePoints; i++)
        {
            curvePoints.Add(bezierCurve.GetCurvePoint(i));
        }

        _lineRenderer.positionCount = curvePoints.Count;
        _lineRenderer.SetPositions(curvePoints.ToArray());
    }

    private void ConfigureLineRenderer()
    {
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lineRenderer.textureMode = LineTextureMode.Tile;
        _lineRenderer.material = _lineMaterial;

        _lineRenderer.startWidth = _widthOfDashes;
        _lineRenderer.endWidth = _widthOfDashes;
    }
}