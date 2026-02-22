using UnityEngine;

public class PlayingWindow : StateWindow<PlayingInputState>
{
    [SerializeField] private GameButton _pauseButton;
    [SerializeField] private GameButton _swapAbilityButton;

    public GameButton PauseButton => _pauseButton;

    public GameButton SwapAbilityButton => _swapAbilityButton;
}