using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimeFlowSettings
{
    [Header("Time Management Keys")]
    [SerializeField] private List<TimeButton> _timeButtons;

    [Header("Increase and Decrease")]
    [SerializeField] private TimeButton _decreasedTimeButton;
    [SerializeField] private TimeButton _increasedTimeButton;

    public List<TimeButton> TimeButtons => _timeButtons;

    public TimeButton DecreasedTimeButton => _decreasedTimeButton;

    public TimeButton IncreasedTimeButton => _increasedTimeButton;
}