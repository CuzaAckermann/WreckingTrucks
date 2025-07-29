using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockTracker
{
    private readonly float _coefficientAcceptableAngle;

    private Field _field;
    private Type _detectableType;

    public BlockTracker(float acceptableAngle)
    {
        float angleRadians = acceptableAngle * Mathf.Deg2Rad;
        _coefficientAcceptableAngle = Mathf.Cos(angleRadians);
    }

    public event Action<Block> TargetDetected;

    public void Prepare(Field field, Type detectableType)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _detectableType = detectableType ?? throw new ArgumentNullException(nameof(detectableType));
    }

    public void Scan(Vector3 currentPosition)
    {
        if (TryGetTargetBlock(currentPosition, out Block target))
        {
            TargetDetected?.Invoke(target);
        }
    }

    private bool TryGetTargetBlock(Vector3 currentPosition, out Block detectableBlock)
    {
        detectableBlock = null;

        if (TryGetAvailableTargets(currentPosition, out List<Block> availableTargets) == false)
        {
            return false;
        }

        if (TrySelectTarget(currentPosition, availableTargets, out detectableBlock) == false)
        {
            return false;
        }

        return true;
    }

    private bool TryGetAvailableTargets(Vector3 currentPosition, out List<Block> availableTargets)
    {
        availableTargets = new List<Block>();
        List<Model> firstModels = _field.GetFirstModels();

        for (int i = 0; i < firstModels.Count; i++)
        {
            if (firstModels[i] is Block block)
            {
                if (block.GetType() != _detectableType)
                {
                    continue;
                }

                if (block.IsTargetForShooting)
                {
                    continue;
                }

                if (Vector3.Dot(Vector3.forward, (block.Position - currentPosition).normalized) < _coefficientAcceptableAngle)
                {
                    continue;
                }

                availableTargets.Add(block);
            }
        }

        return availableTargets.Count > 0;
    }

    private bool TrySelectTarget(Vector3 currentPosition, List<Block> detectableBlocks, out Block target)
    {
        target = detectableBlocks[0];

        for (int i = 0; i < detectableBlocks.Count; i++)
        {
            float sqrMagnitudeToDetectableModel = (detectableBlocks[i].Position - currentPosition).sqrMagnitude;
            float sqrMagnitudeToNearestModel = (target.Position - currentPosition).sqrMagnitude;

            if (sqrMagnitudeToDetectableModel < sqrMagnitudeToNearestModel)
            {
                target = detectableBlocks[i];
            }
        }

        return target != null;
    }
}