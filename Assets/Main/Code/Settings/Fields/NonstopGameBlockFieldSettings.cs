using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NonstopGameBlockFieldSettings : FieldSettings
{
    [SerializeField] private List<ColorType> _generatedColorTypes;

    public IReadOnlyList<ColorType> GeneratedColorTypes => _generatedColorTypes;
}