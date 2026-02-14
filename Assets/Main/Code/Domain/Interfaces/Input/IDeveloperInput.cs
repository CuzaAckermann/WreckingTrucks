public interface IDeveloperInput : IInput
{
    public IInputButton ResetSceneButton { get; }

    public IInputButton SwitchUiButton { get; }

    public ITimeFlowInput TimeFlowInput { get; }
}