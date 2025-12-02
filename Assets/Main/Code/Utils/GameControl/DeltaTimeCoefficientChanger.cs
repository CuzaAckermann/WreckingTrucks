using System;
using System.Collections.Generic;
using UnityEngine;

public class DeltaTimeCoefficientDefiner
{
    private readonly List<TimeButton> _timeButtons;

    public DeltaTimeCoefficientDefiner(List<TimeButton> timeButtons)
    {
        if (timeButtons == null)
        {
            throw new ArgumentNullException(nameof(timeButtons));
        }

        if (timeButtons.Count == 0)
        {
            throw new ArgumentException($"{nameof(timeButtons)} is empty");
        }

        _timeButtons = timeButtons ?? throw new ArgumentNullException(nameof(timeButtons));
        DeltaTimeCoefficient = _timeButtons[2].TimeCoefficient;
    }

    public float DeltaTimeCoefficient { get; private set; }

    public void Update()
    {
        for (int i = 0; i < _timeButtons.Count; i++)
        {
            if (Input.GetKeyDown(_timeButtons[i].Button))
            {
                DeltaTimeCoefficient = _timeButtons[i].TimeCoefficient;
            }
        }
    }
}