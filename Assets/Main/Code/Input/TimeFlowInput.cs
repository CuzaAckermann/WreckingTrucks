using System.Collections.Generic;

public class TimeFlowInput : ITimeFlowInput
{
    public TimeFlowInput(List<ValueButton> timeButtons,
                         ValueButton timeSlowDownButton,
                         ValueButton timeSpeedUpButton)
    {
        Validator.ValidateNotNull(timeButtons, timeSlowDownButton, timeSpeedUpButton);
        Validator.ValidateNotEmpty(timeButtons);

        TimeButtons = timeButtons;
        TimeSlowDownButton = timeSlowDownButton;
        TimeSpeedUpButton = timeSpeedUpButton;
    }

    public IReadOnlyList<ValueButton> TimeButtons { get; private set; }

    public ValueButton TimeSlowDownButton { get; private set; }

    public ValueButton TimeSpeedUpButton { get; private set; }
}