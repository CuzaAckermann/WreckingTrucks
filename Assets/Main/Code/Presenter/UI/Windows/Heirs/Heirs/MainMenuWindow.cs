using UnityEngine;

public class MainMenuWindow : StateWindow<MainMenuInputState>
{
    [SerializeField] private GameButton _hideMenuButton;
    [SerializeField] private GameButton _playButton;
    [SerializeField] private GameButton _optionsButton;
    [SerializeField] private GameButton _shopButton;

    public GameButton HideMenuButton => _hideMenuButton;

    public GameButton PlayButton => _playButton;

    public GameButton OptionsButton => _optionsButton;

    public GameButton ShopButton => _shopButton;
}