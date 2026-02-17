using System;

public interface IWindowsStorage
{
    public BackgroundGameWindow BackgroundGameWindow { get; }

    public MainMenuWindow MainMenuWindow { get; }

    public GameSelectionWindow GameSelectionWindow { get; }

    public LevelButtonsStorage LevelButtonsStorage { get; }

    public OptionsMenu OptionsMenu { get; }

    public ShopWindow ShopWindow { get; }

    public PlayingWindow PlayingWindow { get; }

    public PauseMenu PauseMenu { get; }

    public EndLevelWindow EndLevelWindow { get; }

    public SwapAbilityWindow SwapAbilityWindow { get; }
}