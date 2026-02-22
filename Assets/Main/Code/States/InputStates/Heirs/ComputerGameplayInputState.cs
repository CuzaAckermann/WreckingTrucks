using System.Collections.Generic;

public class ComputerGameplayInputState : InputState
{
    public ComputerGameplayInputState(IInput input) : base(input)
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