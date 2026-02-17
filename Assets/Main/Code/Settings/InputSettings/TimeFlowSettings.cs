using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimeFlowSettings
{
    [Header("Time Factor Buttons")]
    [SerializeField] private List<ValueButtonSettings> _timeFactorButtons;

    [Header("Slow Down and Speed Up Buttons")]
    [SerializeField] private ValueButtonSettings _timeSlowDownButton;
    [SerializeField] private ValueButtonSettings _timeSpeedUpButton;

    public List<ValueButtonSettings> TimeFactorButtons => _timeFactorButtons;

    public ValueButtonSettings TimeSlowDownButton => _timeSlowDownButton;

    public ValueButtonSettings TimeSpeedUpButton => _timeSpeedUpButton;
}