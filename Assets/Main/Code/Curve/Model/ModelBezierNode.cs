using UnityEngine;

public class ModelBezierNode
{
    public ModelBezierNode(Vector3 point, Vector3 tangentIn)
    {
        Point = point;
        TangentIn = tangentIn;
        TangentOut = GetPositionOppositeTangent(TangentIn);
    }

    public Vector3 Point { get; private set; }

    public Vector3 TangentIn { get; private set; }

    public Vector3 TangentOut { get; private set; }

    private Vector3 GetPositionOppositeTangent(Vector3 sourceTangent)
    {
        Vector3 sourceTangentDirection = sourceTangent - Point;
        return Point - sourceTangentDirection;
    }
}