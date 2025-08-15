using System;
using UnityEngine;

[Serializable]
public class BlockTrackerSettings
{
    [SerializeField] private float _range;

    public float Range => _range;
}