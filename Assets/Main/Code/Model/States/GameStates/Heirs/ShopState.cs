using System.Collections.Generic;

public class ShopState : InputState<IInput>
{
    public ShopState(IInput input) : base(input)
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