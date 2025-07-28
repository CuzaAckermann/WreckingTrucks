using System;
using UnityEngine;

[Serializable]
public class BlockTrackerSettings
{
    [SerializeField] private float _acceptableAngle;

    public float AcceptableAngle => _acceptableAngle;
}