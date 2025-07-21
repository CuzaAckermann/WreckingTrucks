using System;
using UnityEngine;

[Serializable]
public class BlockTrackerCreatorSettings
{
    [SerializeField] private float _acceptableAngle;

    public float AcceptableAngle => _acceptableAngle;
}