using UnityEngine;

public class PauseMenu : StateWindow<PausedInputState>
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _returnButton;
    [SerializeField] private GameButton _resetLevelButton;
    [SerializeField] private GameButton _levelSelectionButton;

    public GameButton MainMenuButton => _mainMenuButton;

    public GameButton ReturnButton => _returnButton;

    public GameButton ResetLevelButton => _resetLevelButton;

    public GameButton LevelSelectionButton => _levelSelectionButton;
}