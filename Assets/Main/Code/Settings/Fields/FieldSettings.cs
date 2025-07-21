using System;
using UnityEngine;

[Serializable]
public abstract class FieldSettings
{
    [SerializeField] private FieldSize _fieldSize;

    public FieldSize FieldSize => _fieldSize;
}