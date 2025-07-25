using System;
using UnityEngine;

[Serializable]
public class TruckSpaceSettings : SpaceSettings<TruckFieldSettings>
{
    [SerializeField] private TruckTypeGeneratorSettings _truckTypeGeneratorSettings;

    public TruckTypeGeneratorSettings TruckTypeGeneratorSettings => _truckTypeGeneratorSettings;
}