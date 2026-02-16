public class InputStateStorage
{
    public InputStateStorage(IInput input)
    {
        Validator.ValidateNotNull(input);

        DeveloperInputState = new DeveloperInputState(input);
        ComputerGameplayInputState = new ComputerGameplayInputState(input);
        MainMenuInputState = new MainMenuInputState(input);
        GameSelectionInputState = new GameSelectionInputState(input);
        LevelSelectionInputState = new LevelSelectionInputState(input);
        OptionsMenuInputState = new OptionsMenuInputState(input);
        ShopInputState = new ShopInputState(input);
        PlayingInputState = new PlayingInputState(input);
        SwapAbilityInputState = new SwapAbilityInputState(input);
        PausedInputState = new PausedInputState(input);
        EndLevelInputState = new EndLevelInputState(input);
    }

    public DeveloperInputState DeveloperInputState { get; set; }

    public ComputerGameplayInputState ComputerGameplayInputState { get; }

    public MainMenuInputState MainMenuInputState { get; }

    public GameSelectionInputState GameSelectionInputState { get; }

    public LevelSelectionInputState LevelSelectionInputState { get; }

    public OptionsMenuInputState OptionsMenuInputState { get; }

    public ShopInputState ShopInputState { get; }

    public PlayingInputState PlayingInputState { get; }

    public SwapAbilityInputState SwapAbilityInputState { get; }

    public PausedInputState PausedInputState { get; }

    public EndLevelInputState EndLevelInputState { get; }
}