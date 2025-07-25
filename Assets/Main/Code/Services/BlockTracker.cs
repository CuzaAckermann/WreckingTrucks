using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockTracker
{
    private readonly float _coefficientAcceptableAngle;
    private readonly Vector3 _detectableDirection;
    private readonly Vector3 _undecetableDirection;

    private Field _field;
    private Type _detectableType;
    private bool _isDetectField;
    private int _currentIndexColumn;

    public BlockTracker(float acceptableAngle)
    {
        float angleRadians = acceptableAngle * Mathf.Deg2Rad;
        _coefficientAcceptableAngle = Mathf.Cos(angleRadians);
        _detectableDirection = Quaternion.Euler(0, acceptableAngle, 0) * Vector3.forward;
        _undecetableDirection = Quaternion.Euler(0, -acceptableAngle, 0) * Vector3.forward;
    }

    public event Action<Block> NearestBlockDetected;
    public event Action FieldEscaped;

    public void Prepare(Field field, Type detectableType)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _detectableType = detectableType ?? throw new ArgumentNullException(nameof(detectableType));
        _isDetectField = false;
        _currentIndexColumn = 0;
    }

    public void Scan(Vector3 currentPosition)
    {
        if (_isDetectField == false)
        {
            DetectField(currentPosition);
            return;
        }

        if (TryGetDetectableBlocks(currentPosition, out List<Block> detectableBlocks))
        {
            Block nearestBlock = detectableBlocks[0];

            for (int i = 0; i < detectableBlocks.Count; i++)
            {
                if (detectableBlocks[i].Position.sqrMagnitude < nearestBlock.Position.sqrMagnitude)
                {
                    nearestBlock = detectableBlocks[i];
                }
            }

            NearestBlockDetected?.Invoke(nearestBlock);
        }

        if (_isDetectField)
        {
            DetectEscapeField(currentPosition);
        }
    }

    private bool TryGetDetectableBlocks(Vector3 currentPosition, out List<Block> detectableBlocks)
    {
        List<Model> firstModels = _field.GetFirstModels();
        detectableBlocks = new List<Block>();

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

                detectableBlocks.Add(block);
            }
        }

        return detectableBlocks.Count > 0;
    }

    private void DetectField(Vector3 currentPosition)
    {
        if (_currentIndexColumn < 0 || _currentIndexColumn >= _field.AmountColumns)
        {
            _currentIndexColumn = 0;
        }

        for (int i = 0; i < _field.AmountLayers; i++)
        {
            if (_field.TryGetFirstModel(i, _currentIndexColumn, out Model model))
            {
                if (Vector3.Cross(_detectableDirection, (model.Position - currentPosition).normalized).y <= 0)
                {
                    _isDetectField = true;
                    return;
                }
            }
        }

        if (_isDetectField == false)
        {
            _currentIndexColumn++;
        }
    }

    private void DetectEscapeField(Vector3 currentPosition)
    {
        for (int i = 0; i < _field.AmountLayers; i++)
        {
            if (_field.TryGetFirstModel(i, _field.AmountColumns - 1, out Model model))
            {
                if (Vector3.Cross(_undecetableDirection, (model.Position - currentPosition).normalized).y <= 0)
                {
                    FieldEscaped?.Invoke();
                    return;
                }
            }
        }
    }
}