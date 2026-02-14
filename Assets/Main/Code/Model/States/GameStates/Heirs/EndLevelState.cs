using System.Collections.Generic;

public class EndLevelState : InputState<IInput>
{
    public EndLevelState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        return new List<IInputButton>()
        {
            input.InteractButton
        };
    }
}