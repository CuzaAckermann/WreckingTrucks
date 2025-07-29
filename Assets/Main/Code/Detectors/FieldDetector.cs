using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldDetector
{
    private readonly Vector3 _detectableDirection;
    private readonly Vector3 _undecetableDirection;

    private Field _field;
    private bool _isDetectField;

    public FieldDetector(float acceptableAngle)
    {
        _detectableDirection = Quaternion.Euler(0, acceptableAngle, 0) * Vector3.forward;
        _undecetableDirection = Quaternion.Euler(0, -acceptableAngle, 0) * Vector3.forward;
    }

    public event Action FieldDetected;
    public event Action FieldLeaved;

    public void Prepare(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _isDetectField = false;
    }

    public void Scan(Vector3 currentPosition)
    {
        if (_isDetectField == false)
        {
            DetectEntranceField(currentPosition);
            return;
        }

        if (_isDetectField)
        {
            DetectLeavingField(currentPosition);
        }
    }

    private void DetectEntranceField(Vector3 currentPosition)
    {
        for (int j = 0; j < _field.AmountColumns; j++)
        {
            for (int i = 0; i < _field.AmountLayers; i++)
            {
                if (_field.TryGetFirstModel(i, j, out Model model))
                {
                    if (Vector3.Cross(_detectableDirection, (model.Position - currentPosition).normalized).y <= 0)
                    {
                        _isDetectField = true;
                        FieldDetected?.Invoke();
                        return;
                    }
                }
            }
        }
    }

    private void DetectLeavingField(Vector3 currentPosition)
    {
        for (int i = 0; i < _field.AmountLayers; i++)
        {
            if (_field.TryGetFirstModel(i, _field.AmountColumns - 1, out Model model))
            {
                if (Vector3.Cross(_undecetableDirection, (model.Position - currentPosition).normalized).y <= 0)
                {
                    FieldLeaved?.Invoke();
                    return;
                }
            }
        }
    }
}