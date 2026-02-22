using System.Collections.Generic;

public class DeveloperInputState : InputState
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
            input.TimeFlowInput.TimeSlowDownButton,
            input.TimeFlowInput.TimeSpeedUpButton
        };

        buttons.AddRange(input.TimeFlowInput.TimeButtons);

        return buttons;
    }
}