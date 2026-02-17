using System.Collections.Generic;

public class LevelSelectionInputState : InputState<IInput>
{
    public LevelSelectionInputState(IInput input) : base(input)
    {

    }

    protected override List<IInputButton> GetRequiredButtons(IInput input)
    {
        return new List<IInputButton>()
        {
            // ƒŒ–¿¡Œ“¿“‹
        };
    }
}