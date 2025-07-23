using System;
using UnityEngine;

[Serializable]
public class DeltaTimeCalculator
{
    [Header("Time Scales")]
    [SerializeField, Range(0.001f, 0.1f)] private float _slowTime = 0.1f;
    [SerializeField, Range(0.1f, 1)] private float _middleTime = 1;
    [SerializeField, Range(1, 100)] private float _fastTime = 5;

    public float GetDeltaTime()
    {
        return Time.deltaTime *
               _slowTime *
               _middleTime *
               _fastTime;
    }
}