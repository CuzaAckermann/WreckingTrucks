using System.Collections.Generic;

public class GameSelectionInputState : InputState<IInput>
{
    public GameSelectionInputState(IInput input) : base(input)
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