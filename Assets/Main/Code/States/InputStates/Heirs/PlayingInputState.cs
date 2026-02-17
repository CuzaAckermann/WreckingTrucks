using System.Collections.Generic;

public class PlayingInputState : InputState<IInput>
{
    public PlayingInputState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        return new List<IInputButton>()
        {
            input.InteractButton,
            input.PauseButton
        };
    }
}