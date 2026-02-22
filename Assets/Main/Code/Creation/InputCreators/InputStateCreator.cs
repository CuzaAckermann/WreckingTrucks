using System.Collections.Generic;

public class InputStateCreator
{
    private readonly IInput _input;

    public InputStateCreator(IInput input)
    {
        Validator.ValidateNotNull(input);

        _input = input;
    }

    public List<InputState> Create()
    {
        return new List<InputState>
        {
            new DeveloperInputState(_input),
            new ComputerGameplayInputState(_input),
            new MainMenuInputState(_input),
            new GameSelectionInputState(_input),
            new LevelSelectionInputState(_input),
            new OptionsMenuInputState(_input),
            new ShopInputState(_input),
            new PlayingInputState(_input),
            new SwapAbilityInputState(_input),
            new PausedInputState(_input),
            new EndLevelInputState(_input)
        };
    }
}