public interface ILevelSelectionWindowsStorage
{
    public GameSelectionWindow GameSelectionWindow { get; }

    public LevelButtonsStorage LevelButtonsStorage { get; }

    public PauseMenu PauseMenu { get; }

    public EndLevelWindow EndLevelWindow { get; }
}