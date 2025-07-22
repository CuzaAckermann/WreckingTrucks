using System;
using UnityEngine;

[Serializable]
public abstract class FieldSettings
{
    [SerializeField] private Transform _transform;
    [SerializeField] private FieldSize _fieldSize;

    public Transform Transform => _transform;

    public FieldSize FieldSize => _fieldSize;

    public void SetTransform(Transform transform)
    {
        _transform = transform;
    }
}