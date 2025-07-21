using System;
using UnityEngine;

[Serializable]
public class ApplicationConfigurator
{
    [Header("Application Settings")]
    [SerializeField, Range(30, 300)] private int _targetFrameRate = 60;

    public void ConfigureApplication()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}