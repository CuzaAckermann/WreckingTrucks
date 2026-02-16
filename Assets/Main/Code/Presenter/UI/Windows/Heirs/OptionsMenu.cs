using UnityEngine;

public class OptionsMenu : WindowOfState<OptionsMenuInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}