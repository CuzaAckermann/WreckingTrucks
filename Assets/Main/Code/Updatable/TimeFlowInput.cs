public class TimeFlowInput : ITimeFlowInput
{
    public TimeFlowInput(IInputButton verySlowTimeButton,
                         IInputButton slowTimeButton,
                         IInputButton normalTimeButton,
                         IInputButton fastTimeButton,
                         IInputButton veryFastTimeButton,
                         IInputButton decreasedTimeButton,
                         IInputButton increasedTimeButton)
    {
        VerySlowTimeButton = verySlowTimeButton;
        SlowTimeButton = slowTimeButton;
        NormalTimeButton = normalTimeButton;
        FastTimeButton = fastTimeButton;
        VeryFastTimeButton = veryFastTimeButton;
        DecreasedTimeButton = decreasedTimeButton;
        IncreasedTimeButton = increasedTimeButton;
    }

    public IInputButton VerySlowTimeButton { get; private set; }

    public IInputButton SlowTimeButton { get; private set; }

    public IInputButton NormalTimeButton { get; private set; }

    public IInputButton FastTimeButton { get; private set; }

    public IInputButton VeryFastTimeButton { get; private set; }

    public IInputButton DecreasedTimeButton { get; private set; }

    public IInputButton IncreasedTimeButton { get; private set; }
}