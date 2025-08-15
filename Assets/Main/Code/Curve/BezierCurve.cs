using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private List<BezierNode> _nodes = new List<BezierNode>();
    [SerializeField] private int _segmentsPerSegment = 30;
    [SerializeField] private bool _closedLoop = false;

    [Header("Visualization")]
    [SerializeField] private Color _curveColor = Color.green;
    [SerializeField] private Color _nodesColor = Color.red;
    [SerializeField] private Color _tangentsColor = Color.blue;
    [SerializeField] private float _nodeSize = 0.2f;
    [SerializeField] private float _tangentSize = 0.15f;
    [SerializeField] private bool _showTangents = true;

    [Header("Points")]
    [SerializeField] private List<Vector3> _curvePoints = new List<Vector3>();

    private const int MinAllowedAmountNodes = 2;

    private float[] _segmentLengths;
    private float _totalLength;

    public Color NodesColor => _nodesColor;

    public Color TangentsColor => _tangentsColor;

    public bool ClosedLoop => _closedLoop;

    public IReadOnlyList<BezierNode> Nodes => _nodes;

    public List<Vector3> CurvePoints => _curvePoints;

    public void Initialize()
    {
        if (_nodes == null)
        {
            throw new ArgumentNullException(nameof(_nodes));
        }

        if (_nodes.Count < MinAllowedAmountNodes)
        {
            throw new InvalidOperationException(nameof(_nodes.Count));
        }

        SyncAllTangents();
        CalculateCurve();
    }

    private void Update()
    {
        SyncAllTangents();
        CalculateCurve();
    }

    private void OnDrawGizmos()
    {
        if (_nodes == null || _nodes.Count < MinAllowedAmountNodes)
        {
            return;
        }

        Gizmos.color = _curveColor;

        for (int i = 0; i < _curvePoints.Count - 1; i++)
        {
            Gizmos.DrawLine(_curvePoints[i], _curvePoints[i + 1]);
        }

        foreach (var node in _nodes)
        {
            if (node.Point == null)
            {
                continue;
            }

            Gizmos.color = _nodesColor;
            Gizmos.DrawSphere(node.Point.position, _nodeSize);

            if (_showTangents == false)
            {
                continue;
            }

            if (node.TangentOut != null)
            {
                Gizmos.color = _tangentsColor;
                Gizmos.DrawSphere(node.TangentOut.position, _tangentSize);
                Gizmos.DrawLine(node.Point.position, node.TangentOut.position);
            }

            if (node.TangentIn != null)
            {
                Gizmos.color = _tangentsColor;
                Gizmos.DrawSphere(node.TangentIn.position, _tangentSize);
                Gizmos.DrawLine(node.Point.position, node.TangentIn.position);
            }
        }
    }

    [ContextMenu("Add Node")]
    public void AddNode()
    {
        Vector3 position = _nodes.Count > 0 ?
                           _nodes[_nodes.Count - 1].Point.position + Vector3.right * 2f :
                           transform.position;

        AddNodeAtPosition(position);
    }

    [ContextMenu("Remove Last Node")]
    public void RemoveLastNode()
    {
        RemoveNode(_nodes.Count - 1);
    }

    public void AddNodeAtPosition(Vector3 position)
    {
        GameObject nodeObj = new GameObject("Node_" + _nodes.Count);
        nodeObj.transform.SetParent(transform);
        nodeObj.transform.position = position;

        _nodes.Add(new BezierNode(nodeObj.transform,
                                  CreateTangent(nodeObj.transform, "TangentIn", Vector3.left),
                                  CreateTangent(nodeObj.transform, "TangentOut", Vector3.right),
                                  tangentLength: 1f));
        SyncAllTangents();
    }

    public void RemoveNode(int index)
    {
        if (index < 0 || index >= _nodes.Count)
        {
            return;
        }

        BezierNode node = _nodes[index];

        if (node.Point != null)
        {
            if (Application.isPlaying)
            {
                Destroy(node.Point.gameObject);
            }
            else
            {
                DestroyImmediate(node.Point.gameObject);
            }
        }

        _nodes.RemoveAt(index);
    }

    public bool TryGetFirstNode(out BezierNode node)
    {
        node = null;

        if (_nodes[0] != null)
        {
            node = _nodes[0];
        }

        return node != null;
    }

    public Vector3 GetPointOnCurve(float normalizedPosition)
    {
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        if (normalizedPosition == 0f)
        {
            return _curvePoints[0];
        }
        else if (normalizedPosition == 1f)
        {
            return _curvePoints[_curvePoints.Count - 1];
        }

        float targetLength = _totalLength * normalizedPosition;
        float accumulatedLength = 0f;

        for (int i = 0; i < _segmentLengths.Length; i++)
        {
            if (accumulatedLength + _segmentLengths[i] >= targetLength)
            {
                float remainingLength = targetLength - accumulatedLength;
                float t = remainingLength / _segmentLengths[i];
                return Vector3.Lerp(_curvePoints[i], _curvePoints[i + 1], t);
            }

            accumulatedLength += _segmentLengths[i];
        }

        return _curvePoints[_curvePoints.Count - 1];
    }

    public void SyncAllTangents()
    {
        foreach (var node in _nodes)
        {
            node?.SyncTangents();
        }
    }

    public void CalculateCurve()
    {
        _curvePoints.Clear();

        if (_nodes.Count < MinAllowedAmountNodes)
        {
            return;
        }

        int segments = _closedLoop ? _nodes.Count : _nodes.Count - 1;

        for (int i = 0; i < segments; i++)
        {
            int currentIndex = i;
            int nextIndex = (i + 1) % _nodes.Count;

            BezierNode currentNode = _nodes[currentIndex];
            BezierNode nextNode = _nodes[nextIndex];

            if (currentNode.Point == null || nextNode.Point == null)
            {
                continue;
            }

            Vector3 point0 = currentNode.Point.position;
            Vector3 point1 = currentNode.TangentOut.position;
            Vector3 point2 = nextNode.TangentIn.position;
            Vector3 point3 = nextNode.Point.position;

            for (int j = 0; j <= _segmentsPerSegment; j++)
            {
                float step = j / (float)_segmentsPerSegment;
                _curvePoints.Add(CalculateBezierPoint(step, point0, point1, point2, point3));
            }
        }

        CacheLengths();
    }

    private void CacheLengths()
    {
        _segmentLengths = new float[_curvePoints.Count - 1];
        _totalLength = 0f;

        for (int i = 0; i < _segmentLengths.Length; i++)
        {
            _segmentLengths[i] = Vector3.Distance(_curvePoints[i], _curvePoints[i + 1]);
            _totalLength += _segmentLengths[i];
        }
    }

    private Vector3 CalculateBezierPoint(float step,
                                         Vector3 point0,
                                         Vector3 point1,
                                         Vector3 point2,
                                         Vector3 point3)
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

    private Transform CreateTangent(Transform parent, string name, Vector3 offset)
    {
        GameObject tangent = new GameObject(name);
        tangent.transform.SetParent(parent);
        tangent.transform.localPosition = offset;
        return tangent.transform;
    }
}