using System;

public class InputStateStorage
{
    public InputStateStorage(ComputerGameplayInputState backgroundGameState,
                             MainMenuInputState mainMenuState,
                             GameSelectionInputState gameSelectionState,
                             LevelSelectionInputState levelSelectionState,
                             OptionsMenuState optionsMenuState,
                             ShopState shopState,
                             PlayingInputState playingState,
                             SwapAbilityInputState swapAbilityState,
                             PausedInputState pausedState,
                             EndLevelState endLevelState)
    {
        BackgroundGameState = backgroundGameState ?? throw new ArgumentNullException(nameof(backgroundGameState));
        MainMenuState = mainMenuState ?? throw new ArgumentNullException(nameof(mainMenuState));
        GameSelectionState = gameSelectionState ?? throw new ArgumentNullException(nameof(gameSelectionState));
        LevelSelectionState = levelSelectionState ?? throw new ArgumentNullException(nameof(levelSelectionState));
        OptionsMenuState = optionsMenuState ?? throw new ArgumentNullException(nameof(optionsMenuState));
        ShopState = shopState ?? throw new ArgumentNullException(nameof(shopState));
        PlayingState = playingState ?? throw new ArgumentNullException(nameof(playingState));
        SwapAbilityState = swapAbilityState ?? throw new ArgumentNullException(nameof(swapAbilityState));
        PausedState = pausedState ?? throw new ArgumentNullException(nameof(pausedState));
        EndLevelState = endLevelState ?? throw new ArgumentNullException(nameof(endLevelState));
    }

    public ComputerGameplayInputState BackgroundGameState { get; }

    public MainMenuInputState MainMenuState { get; }

    public GameSelectionInputState GameSelectionState { get; }

    public LevelSelectionInputState LevelSelectionState { get; }

    public OptionsMenuState OptionsMenuState { get; }

    public ShopState ShopState { get; }

    public PlayingInputState PlayingState { get; }

    public SwapAbilityInputState SwapAbilityState { get; }

    public PausedInputState PausedState { get; }

    public EndLevelState EndLevelState { get; }
}