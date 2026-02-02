using UnityEngine;

public class EndLevelWindow : WindowOfState<EndLevelState>
{
    [Header("Main Buttons")]
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _resetLevelButton;

    [Header("Level Buttons")]
    [SerializeField] private GameButton _levelSelectionButton;
    [SerializeField] private GameButton _previousLevelButton;
    [SerializeField] private GameButton _nextLevelButton;

    public GameButton MainMenuButton => _mainMenuButton;

    public GameButton ResetLevelButton => _resetLevelButton;

    public GameButton LevelSelectionButton => _levelSelectionButton;

    public GameButton PreviousLevelButton => _previousLevelButton;

    public GameButton NextLevelButton => _nextLevelButton;

    public void SetLevelNavigationState(bool hasNextLevel, bool hasPreviousLevel)
    {
        _nextLevelButton.gameObject.SetActive(hasNextLevel);
        _previousLevelButton.gameObject.SetActive(hasPreviousLevel);
    }
}