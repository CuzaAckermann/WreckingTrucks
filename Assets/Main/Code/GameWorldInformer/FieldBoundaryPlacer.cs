using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FieldBoundaryPlacer
{
    public ModelBezierCurve PlaceBezierCurve(Field field, BorderSettings bezierCurveSettings, float height)
    {
        if (bezierCurveSettings == null)
        {
            throw new ArgumentNullException(nameof(bezierCurveSettings));
        }

        ModelBezierCurve bezierCurve = new ModelBezierCurve(bezierCurveSettings.SegmentsPerSegment,
                                                                  bezierCurveSettings.IsLoop);
        List<ModelBezierNode> nodes = CreateBezierNodes(field, bezierCurveSettings, height);

        for (int i = 0; i < nodes.Count; i++)
        {
            bezierCurve.AddNode(nodes[i]);
        }

        bezierCurve.CalculateCurve();

        return bezierCurve;
    }

    private List<ModelBezierNode> CreateBezierNodes(Field field, BorderSettings bezierCurveSettings, float height)
    {
        if (field == null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        List<ModelBezierNode> nodes = new List<ModelBezierNode>();

        Vector3 lengthOfField = field.Forward * field.IntervalBetweenRows * (field.AmountRows - 1);
        Vector3 widthOfField = field.Right * field.IntervalBetweenColumns * (field.AmountColumns - 1);

        Vector3 currentPoint = field.Position + lengthOfField + -field.Right * bezierCurveSettings.Offset;

        currentPoint = new Vector3(currentPoint.x,
                                   height,
                                   currentPoint.z);

        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + field.Forward * bezierCurveSettings.TangentLength));

        currentPoint += -lengthOfField;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + field.Forward * bezierCurveSettings.TangentLength * field.IntervalBetweenRows));

        currentPoint += field.Right * bezierCurveSettings.Offset + -field.Forward * bezierCurveSettings.Offset * field.IntervalBetweenRows;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -field.Right * bezierCurveSettings.TangentLength));

        currentPoint += widthOfField;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -field.Right * bezierCurveSettings.TangentLength));

        currentPoint += field.Right * bezierCurveSettings.Offset + field.Forward * bezierCurveSettings.Offset;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -field.Forward * bezierCurveSettings.TangentLength));

        currentPoint += lengthOfField;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -field.Forward * bezierCurveSettings.TangentLength));

        if (bezierCurveSettings.IsLoop == false)
        {
            return nodes;
        }

        currentPoint += field.Forward * bezierCurveSettings.Offset + -field.Right * bezierCurveSettings.Offset;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + field.Right * bezierCurveSettings.TangentLength));

        currentPoint += -widthOfField;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + field.Right * bezierCurveSettings.TangentLength));

        return nodes;
    }
}