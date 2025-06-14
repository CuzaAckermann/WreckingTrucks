using System;
using UnityEngine;

[Serializable]
public class FactorySettings
{
    [SerializeField] private int _initialPoolSize;
    [SerializeField] private int _maxPoolCapacity;

    public FactorySettings(int initialPoolSize, int maxPoolCapacity)
    {
        if (initialPoolSize <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(initialPoolSize)} must be positive.");
        }

        if (maxPoolCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(maxPoolCapacity)} must be positive.");
        }

        _initialPoolSize = initialPoolSize;
        _maxPoolCapacity = maxPoolCapacity;
    }

    public int InitialPoolSize => _initialPoolSize;

    public int MaxPoolCapacity => _maxPoolCapacity;
}