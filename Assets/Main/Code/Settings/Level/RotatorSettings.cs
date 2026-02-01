using System;
using UnityEngine;

[Serializable]
public class RotatorSettings
{
    [SerializeField, Min(1)] private int _capacityRotatables;

    public int CapacityRotatables => _capacityRotatables;
}