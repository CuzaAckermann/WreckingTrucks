using System;
using UnityEngine;

[Serializable]
public class RotatorUpdaterSettings
{
    [SerializeField, Min(1)] private int _capacityRotatables;

    public int CapacityRotatables => _capacityRotatables;
}