using UnityEngine;

public class ComputerGameplayWindow : StateWindow<ComputerGameplayInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}