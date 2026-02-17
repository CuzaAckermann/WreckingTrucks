using System.Collections.Generic;

public class OptionsMenuInputState : InputState<IInput>
{
    public OptionsMenuInputState(IInput input) : base(input)
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