using System.Collections.Generic;

public class EndLevelInputState : InputState<IInput>
{
    public EndLevelInputState(IInput input) : base(input)
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