using System.Collections.Generic;

public class ShopInputState : InputState
{
    public ShopInputState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        return new List<IInputButton>()
        {
            // днонкмхрэ
        };
    }
}