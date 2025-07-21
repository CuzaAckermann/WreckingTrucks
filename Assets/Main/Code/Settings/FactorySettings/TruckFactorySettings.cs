using System;
using UnityEngine;

[Serializable]
public class TruckFactorySettings : FactorySettings
{
    [SerializeField] private TruckSettings _truckSettings;

    public TruckSettings TruckSettings => _truckSettings;
}