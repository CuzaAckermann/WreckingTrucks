using System;
using UnityEngine;

[Serializable]
public class MoverSettings
{
    [SerializeField] private int _capacityMoveables;

    // убери это
    public MoverSettings(int capacityMoveables)
    {
        _capacityMoveables = capacityMoveables;
    }
    // убери это

    public int CapacityMoveables => _capacityMoveables;
}