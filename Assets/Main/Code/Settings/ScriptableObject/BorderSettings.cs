using UnityEngine;

[CreateAssetMenu(fileName = "BorderSettings", menuName = "Settings/Border Settings")]
public class BorderSettings : ScriptableObject
{
    [SerializeField] private int _segmentsPerSegment;
    [SerializeField] private bool _isLoop;
    [SerializeField] private float _tangentLength;
    [SerializeField] private float _offset;

    public int SegmentsPerSegment => _segmentsPerSegment;

    public bool IsLoop => _isLoop;

    public float TangentLength => _tangentLength;

    public float Offset => _offset;
}