using System;
using UnityEngine;

[Serializable]
public class FillerSettings
{
    [SerializeField] private float _frequency;

    public float Frequency => _frequency;
}