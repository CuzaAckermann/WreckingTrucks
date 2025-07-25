using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TruckFieldSettings : FieldSettings
{
    [SerializeField] private List<ColorType> _types;

    public IReadOnlyList<ColorType> Types => _types;
}