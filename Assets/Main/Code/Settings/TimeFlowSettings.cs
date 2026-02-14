using System;
using UnityEngine;

[Serializable]
public class TimeFlowSettings
{
    [Header("Time Management Keys")]
    [SerializeField] private TimeButton _verySlowTimeButton;
    [SerializeField] private TimeButton _slowTimeButton;
    [SerializeField] private TimeButton _normalTimeButton;
    [SerializeField] private TimeButton _fastTimeButton;
    [SerializeField] private TimeButton _veryFastTimeButton;

    [Header("Increase and Decrease")]
    [SerializeField] private TimeButton _decreasedTimeButton;
    [SerializeField] private TimeButton _increasedTimeButton;

    public TimeButton VerySlowTimeButton => _verySlowTimeButton;

    public TimeButton SlowTimeButton => _slowTimeButton;

    public TimeButton NormalTimeButton => _normalTimeButton;

    public TimeButton FastTimeButton => _fastTimeButton;

    public TimeButton VeryFastTimeButton => _veryFastTimeButton;

    public TimeButton DecreasedTimeButton => _decreasedTimeButton;

    public TimeButton IncreasedTimeButton => _increasedTimeButton;
}