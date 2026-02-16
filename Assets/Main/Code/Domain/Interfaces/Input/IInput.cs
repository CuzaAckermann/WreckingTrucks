public interface IInput
{
    public IInputButton PauseButton { get; }

    public IInputButton InteractButton { get; }

    public IInputButton ResetSceneButton { get; }

    public IInputButton SwitchUiButton { get; }

    public ITimeFlowInput TimeFlowInput { get; }
}