using System;
using UnityEngine;

[Serializable]
public class PlaneFactorySettings : FactorySettings
{
    [SerializeField] private PlaneSettings _planeSettings;

    public PlaneSettings PlaneSettings => _planeSettings;
}