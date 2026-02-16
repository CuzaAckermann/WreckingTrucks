using System.Collections.Generic;

public class DeveloperInputState : InputState<IInput>
{
    public DeveloperInputState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        List<IInputButton> buttons = new List<IInputButton>()
        {
            input.ResetSceneButton,
            input.SwitchUiButton,
            input.TimeFlowInput.DecreasedTimeButton,
            input.TimeFlowInput.IncreasedTimeButton
        };

        buttons.AddRange(input.TimeFlowInput.TimeButtons);

        return buttons;
    }
}