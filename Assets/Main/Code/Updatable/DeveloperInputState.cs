using System.Collections.Generic;

public class DeveloperInputState : InputState<IDeveloperInput>
{
    public DeveloperInputState(IDeveloperInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IDeveloperInput input)
    {
        return new List<IInputButton>()
        {
            input.ResetSceneButton,
            input.SwitchUiButton,
            input.TimeFlowInput.VerySlowTimeButton,
            input.TimeFlowInput.SlowTimeButton,
            input.TimeFlowInput.NormalTimeButton,
            input.TimeFlowInput.FastTimeButton,
            input.TimeFlowInput.VeryFastTimeButton,
            input.TimeFlowInput.DecreasedTimeButton,
            input.TimeFlowInput.IncreasedTimeButton
        };
    }
}