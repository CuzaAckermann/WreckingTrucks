using UnityEngine;

public class GameSelectionWindow : WindowOfState<GameSelectionState>
{
    [SerializeField] private GameButton _startNewGameButton;
    [SerializeField] private GameButton _continueButton;
    [SerializeField] private GameButton _levelsButton;
    [SerializeField] private GameButton _returnButton;

    public override void Init(GameSelectionState gameState)
    {
        // Temporary
        _startNewGameButton.BecomeInactive();
        _continueButton.BecomeInactive();

        base.Init(gameState);
    }

    public GameButton StartNewGameButton => _startNewGameButton;

    public GameButton ContinueButton => _continueButton;

    public GameButton LevelsButton => _levelsButton;

    public GameButton ReturnButton => _returnButton;
}