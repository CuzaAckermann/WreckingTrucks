using System.Collections.Generic;

public class PausedInputState : InputState
{
    public PausedInputState(IInput input) : base(input)
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