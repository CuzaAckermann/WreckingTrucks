using UnityEngine;

[CreateAssetMenu(fileName = "BezierCurveSettings", menuName = "Settings/Bezier Curve Settings")]
public class BezierCurveSettings : ScriptableObject
{
    [SerializeField] private int _segmentsPerSegment;
    [SerializeField] private bool _isLoop;

    public int SegmentsPerSegment => _segmentsPerSegment;

    public bool IsLoop => _isLoop;
}