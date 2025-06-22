using System;
using UnityEngine;

public class BlockTracker
{
    private Field _field;
    private int _currentIndexColumn;
    private float _coefficientAcceptableAngle;
    private Vector3 _detectableDirection;
    private Vector3 _undecetableDirection;

    private bool _isDetectField;

    public event Action<Block> AcceptableAngleReached;
    public event Action FieldEscaped;

    public void SetField(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _currentIndexColumn = 0;
    }

    public void SetAcceptableAngle(float acceptableAngle)
    {
        float angleRadians = acceptableAngle * Mathf.Deg2Rad;
        _coefficientAcceptableAngle = Mathf.Cos(angleRadians);
        _detectableDirection = Quaternion.Euler(0, acceptableAngle, 0) * Vector3.forward;
        _undecetableDirection = Quaternion.Euler(0, -acceptableAngle, 0) * Vector3.forward;
        _isDetectField = false;
    }

    public void Tick(Vector3 currentPosition)
    {
        if (_isDetectField == false)
        {
            DetectField(currentPosition);
        }
        else
        {
            if (_currentIndexColumn < _field.AmountColumn)
            {
                if (_field.TryGetFirstElement(_currentIndexColumn, out Model model))
                {
                    if (Vector3.Dot(Vector3.forward, (model.Position - currentPosition).normalized) > _coefficientAcceptableAngle)
                    {
                        if (model is Block block)
                        {
                            if (block.IsTargetForShooting == false)
                            {
                                AcceptableAngleReached?.Invoke(block);
                            }
                        }
                    }

                    if (Vector3.Cross(_undecetableDirection, (model.Position - currentPosition).normalized).y < 0)
                    {
                        Logger.Log(5);

                        _currentIndexColumn++;
                    }
                }
                else
                {
                    Logger.Log(6);

                    _currentIndexColumn++;
                }
            }
            else if (_currentIndexColumn == _field.AmountColumn)
            {
                if (_field.TryGetFirstElement(_field.AmountColumn - 1, out Model model))
                {
                    if (Vector3.Cross(_undecetableDirection, (model.Position - currentPosition).normalized).y > 0)
                    {
                        FieldEscaped?.Invoke();
                    }
                }
            }
        }
    }

    private void DetectField(Vector3 currentPosition)
    {
        if (_field.TryGetFirstElement(0, out Model model))
        {
            if (Vector3.Cross(_detectableDirection, (model.Position - currentPosition).normalized).y < 0)
            {
                _isDetectField = true;
                return;
            }
        }
    }
}