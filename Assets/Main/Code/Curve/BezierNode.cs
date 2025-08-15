using System;
using UnityEngine;

[Serializable]
public class BezierNode
{
    [SerializeField] private Transform _point;
    [SerializeField] private Transform _tangentIn;
    [SerializeField] private Transform _tangentOut;
    [SerializeField] private float _tangentLength = 1f;

    public BezierNode(Transform point, Transform tangentIn, Transform tangentOut, float tangentLength)
    {
        if (tangentLength <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(tangentLength));
        }

        _point = point ? point : throw new ArgumentNullException(nameof(point));
        _tangentIn = tangentIn ? tangentIn : throw new ArgumentNullException(nameof(tangentIn));
        _tangentOut = tangentOut ? tangentOut : throw new ArgumentNullException(nameof(tangentOut));
        _tangentLength = tangentLength;
    }

    public Transform Point => _point;

    public Transform TangentIn => _tangentIn;

    public Transform TangentOut => _tangentOut;

    public void SyncTangents()
    {
        if (_point == null ||
            _tangentIn == null ||
            _tangentOut == null)
        {
            return;
        }

        if (_point.hasChanged)
        {
            Vector3 delta = _point.position - _point.transform.position;
            _tangentIn.position += delta;
            _tangentOut.position += delta;
            _point.hasChanged = false;

            return;
        }

        if (_tangentOut.hasChanged)
        {
            ChangeOppositeTangent(_tangentOut, _point.position, _tangentIn);
        }
        else if (_tangentIn.hasChanged)
        {
            ChangeOppositeTangent(_tangentIn, _point.position, _tangentOut);
        }
    }

    private void ChangeOppositeTangent(Transform sourceTangent, Vector3 center, Transform oppositeTangent)
    {
        Vector3 sourceTangentDirection = sourceTangent.position - center;
        oppositeTangent.position = center - sourceTangentDirection;
        _tangentLength = sourceTangentDirection.magnitude;
        sourceTangent.hasChanged = false;
    }
}