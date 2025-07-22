using System;
using UnityEngine;

[Serializable]
public class SwapAbilitySettings
{
    [SerializeField] private MoverSettings _moverSettings;

    public MoverSettings MoverSettings => _moverSettings;
}