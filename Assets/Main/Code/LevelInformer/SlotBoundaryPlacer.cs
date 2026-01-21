using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlotBoundaryPlacer
{
    public ModelBezierCurve PlaceBezierCurve(Transform planeSlot,
                                             SlotBorderSettings slotBorderSettings,
                                             float height)
    {
        if (slotBorderSettings == null)
        {
            throw new ArgumentNullException(nameof(slotBorderSettings));
        }

        ModelBezierCurve bezierCurve = new ModelBezierCurve(slotBorderSettings.SegmentsPerSegment,
                                                            true);
        List<ModelBezierNode> nodes = CreateBezierNodes(planeSlot, slotBorderSettings, height);

        for (int i = 0; i < nodes.Count; i++)
        {
            bezierCurve.AddNode(nodes[i]);
        }

        bezierCurve.CalculateCurve();

        return bezierCurve;
    }

    private List<ModelBezierNode> CreateBezierNodes(Transform planeSlot, SlotBorderSettings slotBorderSettings, float height)
    {
        if (planeSlot == null)
        {
            throw new ArgumentNullException(nameof(planeSlot));
        }

        List<ModelBezierNode> nodes = new List<ModelBezierNode>();

        Vector3 lengthOfSlot = planeSlot.forward * slotBorderSettings.Length;
        Vector3 widthOfSlot = planeSlot.right * slotBorderSettings.Width;

        Vector3 halfLength = lengthOfSlot / 2;
        Vector3 halfWidth = widthOfSlot / 2;

        Vector3 currentPoint = planeSlot.position + halfLength + (halfWidth + -planeSlot.right * slotBorderSettings.TangentLength);

        currentPoint = new Vector3(currentPoint.x,
                                   height,
                                   currentPoint.z);

        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + planeSlot.right * slotBorderSettings.TangentLength));

        currentPoint += -widthOfSlot + planeSlot.right * (slotBorderSettings.TangentLength * 2);
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + planeSlot.right * slotBorderSettings.TangentLength));

        currentPoint += -planeSlot.forward * slotBorderSettings.TangentLength + -planeSlot.right * slotBorderSettings.TangentLength;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + planeSlot.forward * slotBorderSettings.TangentLength));

        currentPoint += -lengthOfSlot + planeSlot.forward * (slotBorderSettings.TangentLength * 2);
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + planeSlot.forward * slotBorderSettings.TangentLength));

        currentPoint += planeSlot.right * slotBorderSettings.TangentLength + -planeSlot.forward * slotBorderSettings.TangentLength;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -planeSlot.right * slotBorderSettings.TangentLength));

        currentPoint += widthOfSlot + -planeSlot.right * (slotBorderSettings.TangentLength * 2);
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -planeSlot.right * slotBorderSettings.TangentLength));

        currentPoint += planeSlot.forward * slotBorderSettings.TangentLength + planeSlot.right * slotBorderSettings.TangentLength;
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -planeSlot.forward * slotBorderSettings.TangentLength));

        currentPoint += lengthOfSlot + -planeSlot.forward * (slotBorderSettings.TangentLength * 2);
        nodes.Add(new ModelBezierNode(currentPoint, currentPoint + -planeSlot.forward * slotBorderSettings.TangentLength));

        return nodes;
    }
}
