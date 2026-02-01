using System;
using UnityEngine;

public class MainMenuWindow : WindowOfState<MainMenuInputState>
{
    [SerializeField] private GameButton _hideMenuButton;
    [SerializeField] private GameButton _playButton;
    [SerializeField] private GameButton _optionsButton;
    [SerializeField] private GameButton _shopButton;

    public event Action HideMenuButtonPressed;
    public event Action PlayButtonPressed;
    public event Action OptionsButtonPressed;
    public event Action ShopButtonPressed;

    protected override void SubscribeToInteractables(MainMenuInputState mainMenuState)
    {
        _hideMenuButton.Pressed += OnHideMenuButtonPressed;
        _playButton.Pressed += OnPlayButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
        _shopButton.Pressed += OnShopButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(MainMenuInputState mainMenuState)
    {
        _hideMenuButton.Pressed -= OnHideMenuButtonPressed;
        _playButton.Pressed -= OnPlayButtonPressed;
        _optionsButton.Pressed -= OnOptionsButtonPressed;
        _shopButton.Pressed -= OnShopButtonPressed;
    }

    private void OnHideMenuButtonPressed()
    {
        HideMenuButtonPressed?.Invoke();
    }

    private void OnPlayButtonPressed()
    {
        PlayButtonPressed?.Invoke();
    }

    private void OnOptionsButtonPressed()
    {
        OptionsButtonPressed?.Invoke();
    }

    private void OnShopButtonPressed()
    {
        ShopButtonPressed?.Invoke();
    }
}