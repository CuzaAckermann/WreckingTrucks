using System.Collections.Generic;

public class OptionsMenuState : InputState<IInput>
{
    public OptionsMenuState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        return new List<IInputButton>()
        {
            input.PauseButton
        };
    }
}