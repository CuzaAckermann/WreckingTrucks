using System;
using UnityEngine;

[Serializable]
public class GunFactorySettings : FactorySettings
{
    [SerializeField] private GunSettings _gunSettings;

    public GunSettings GunSettings => _gunSettings;
}