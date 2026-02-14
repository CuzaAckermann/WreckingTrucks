using UnityEngine;

public class GameSelectionWindow : WindowOfState<GameSelectionInputState>
{
    [SerializeField] private GameButton _startNewGameButton;
    [SerializeField] private GameButton _continueButton;
    [SerializeField] private GameButton _levelsButton;
    [SerializeField] private GameButton _returnButton;

    public GameButton StartNewGameButton => _startNewGameButton;

    public GameButton ContinueButton => _continueButton;

    public GameButton LevelsButton => _levelsButton;

    public GameButton ReturnButton => _returnButton;
}