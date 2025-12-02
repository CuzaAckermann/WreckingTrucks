using System;
using UnityEngine;

[Serializable]
public class BlockTrackerSettings
{
    [SerializeField] private float _detectionInterval;

    public float DetectionInterval => _detectionInterval;
}