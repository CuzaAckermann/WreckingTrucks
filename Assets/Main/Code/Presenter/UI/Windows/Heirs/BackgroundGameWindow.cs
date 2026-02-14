using UnityEngine;

public class BackgroundGameWindow : WindowOfState<ComputerGameplayInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}