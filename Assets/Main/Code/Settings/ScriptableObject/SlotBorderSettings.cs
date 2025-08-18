using UnityEngine;

[CreateAssetMenu(fileName = "SlotBorderSettings", menuName = "Settings/Slot Border Settings")]
public class SlotBorderSettings : ScriptableObject
{
    [Header("Size")]
    [SerializeField] private float _width;
    [SerializeField] private float _length;

    [Header("CurveSettings")]
    [SerializeField] private int _segmentsPerSegment;
    [SerializeField] private float _tangentLength;

    public float Width => _width;

    public float Length => _length;

    public int SegmentsPerSegment => _segmentsPerSegment;

    public float TangentLength => _tangentLength;
}