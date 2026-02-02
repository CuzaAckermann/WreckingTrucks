using UnityEngine;

public class BackgroundGameWindow : WindowOfState<BackgroundGameState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}