using System.Collections.Generic;

public class SwapAbilityInputState : InputState
{
    public SwapAbilityInputState(IInput input)
                     : base(input)
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