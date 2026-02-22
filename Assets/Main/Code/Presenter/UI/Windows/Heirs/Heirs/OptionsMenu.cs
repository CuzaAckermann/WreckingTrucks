using UnityEngine;

public class OptionsMenu : StateWindow<OptionsMenuInputState>
{
    [SerializeField] private GameButton _returnButton;

    public GameButton ReturnButton => _returnButton;
}