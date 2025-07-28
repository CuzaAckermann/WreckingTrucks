using System.Collections.Generic;
using UnityEngine;

public class NearestTargetChooser : ITargetChooser
{
    public Model SelectTarget(Vector3 pointReference, List<Model> detectableModels)
    {
        Model nearestTarget = detectableModels[0];

        for (int i = 0; i < detectableModels.Count; i++)
        {
            float sqrMagnitudeToDetectableModel = (detectableModels[i].Position - pointReference).sqrMagnitude;
            float sqrMagnitudeToNearestModel = (nearestTarget.Position - pointReference).sqrMagnitude;

            if (sqrMagnitudeToDetectableModel < sqrMagnitudeToNearestModel)
            {
                nearestTarget = detectableModels[i];
            }
        }

        return nearestTarget;
    }
}