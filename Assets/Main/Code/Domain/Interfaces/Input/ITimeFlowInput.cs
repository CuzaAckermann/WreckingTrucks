public interface ITimeFlowInput
{
    public IInputButton VerySlowTimeButton { get; }

    public IInputButton SlowTimeButton { get; }

    public IInputButton NormalTimeButton { get; }

    public IInputButton FastTimeButton { get; }

    public IInputButton VeryFastTimeButton { get; }

    public IInputButton DecreasedTimeButton { get; }

    public IInputButton IncreasedTimeButton { get; }
}