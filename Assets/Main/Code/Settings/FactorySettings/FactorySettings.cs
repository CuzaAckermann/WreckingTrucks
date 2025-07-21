using System;
using UnityEngine;

[Serializable]
public class FactorySettings
{
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _maxPoolCapacity;

    public int InitialPoolSize => _initialPoolSize;

    public int MaxPoolCapacity => _maxPoolCapacity;
}