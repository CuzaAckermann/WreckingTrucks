using UnityEngine;

public class GameSelectionWindow : WindowOfState<GameSelectionState>
{
    [SerializeField] private GameButton _startNewGameButton;
    [SerializeField] private GameButton _continueButton;
    [SerializeField] private GameButton _levelSelectionButton;
    [SerializeField] private GameButton _returnButton;

    public override void Init(GameSelectionState gameState)
    {
        // Temporary
        _startNewGameButton.Off();
        _continueButton.Off();

        base.Init(gameState);
    }

    public GameButton StartNewGameButton => _startNewGameButton;

    public GameButton ContinueButton => _continueButton;

    public GameButton LevelSelectionButton => _levelSelectionButton;

    public GameButton ReturnButton => _returnButton;
}