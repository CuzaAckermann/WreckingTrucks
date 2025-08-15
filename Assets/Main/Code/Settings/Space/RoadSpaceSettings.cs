using System;
using UnityEngine;

[Serializable]
public class RoadSpaceSettings
{
    [SerializeField] private MoverSettings _moverSettings;
    [SerializeField] private RotatorSettings _rotatorSettings;

    public MoverSettings MoverSettings => _moverSettings;

    public RotatorSettings RotatorSettings => _rotatorSettings;
}