using System;

public class StateStorage
{
    public StateStorage(BackgroundGameState backgroundGameState,
                        MainMenuInputState mainMenuState,
                        GameSelectionState gameSelectionState,
                        LevelSelectionState levelSelectionState,
                        OptionsMenuState optionsMenuState,
                        ShopState shopState,
                        PlayingInputState playingState,
                        SwapAbilityState swapAbilityState,
                        PausedState pausedState,
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

    public BackgroundGameState BackgroundGameState { get; }

    public MainMenuInputState MainMenuState { get; }

    public GameSelectionState GameSelectionState { get; }

    public LevelSelectionState LevelSelectionState { get; }

    public OptionsMenuState OptionsMenuState { get; }

    public ShopState ShopState { get; }

    public PlayingInputState PlayingState { get; }

    public SwapAbilityState SwapAbilityState { get; }

    public PausedState PausedState { get; }

    public EndLevelState EndLevelState { get; }
}