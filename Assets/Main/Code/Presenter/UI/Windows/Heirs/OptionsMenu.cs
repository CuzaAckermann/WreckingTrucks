using UnityEngine;

public class OptionsMenu : WindowOfState<OptionsMenuState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}