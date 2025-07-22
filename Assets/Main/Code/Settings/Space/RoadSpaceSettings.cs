using System;
using UnityEngine;

[Serializable]
public class RoadSpaceSettings
{
    [SerializeField] private MoverSettings _moverSettings;
    [SerializeField] private RotatorSettings _rotatorSettings;
    [SerializeField] private PathSettings _pathSettings;

    public MoverSettings MoverSettings => _moverSettings;

    public RotatorSettings RotatorSettings => _rotatorSettings;

    public PathSettings PathSettings => _pathSettings;

    public void SetPathSettings(PathSettings pathSettings)
    {
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    }
}