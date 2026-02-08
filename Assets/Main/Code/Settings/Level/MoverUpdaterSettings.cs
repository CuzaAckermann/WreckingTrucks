using System;
using UnityEngine;

[Serializable]
public class MoverUpdaterSettings
{
    [SerializeField] private int _capacityMoveables;

    // убери это
    public MoverUpdaterSettings(int capacityMoveables)
    {
        _capacityMoveables = capacityMoveables;
    }
    // убери это

    public int CapacityMoveables => _capacityMoveables;
}