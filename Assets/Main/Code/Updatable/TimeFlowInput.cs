using System.Collections.Generic;

public class TimeFlowInput : ITimeFlowInput
{
    public TimeFlowInput(List<TimeButton> timeButtons,
                         TimeButton decreasedTimeButton,
                         TimeButton increasedTimeButton)
    {
        Validator.ValidateNotNull(timeButtons, decreasedTimeButton, increasedTimeButton);
        Validator.ValidateNotEmpty(timeButtons);

        TimeButtons = timeButtons;
        DecreasedTimeButton = decreasedTimeButton;
        IncreasedTimeButton = increasedTimeButton;
    }

    public IReadOnlyList<TimeButton> TimeButtons { get; private set; }

    public TimeButton DecreasedTimeButton { get; private set; }

    public TimeButton IncreasedTimeButton { get; private set; }
}