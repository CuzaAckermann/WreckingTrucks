using System;
using UnityEngine;

public class MainMenu : Window
{
    [SerializeField] private GameButton _playButton;
    [SerializeField] private GameButton _optionsButton;
    [SerializeField] private GameButton _hideMenuButton;

    public event Action PlayButtonPressed;
    public event Action OptionsButtonPressed;
    public event Action HideMenuButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _playButton.Pressed += OnPlayButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
        _hideMenuButton.Pressed += OnHideMenuButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _playButton.Pressed -= OnPlayButtonPressed;
        _optionsButton.Pressed -= OnOptionsButtonPressed;
        _hideMenuButton.Pressed -= OnHideMenuButtonPressed;
    }

    private void OnPlayButtonPressed()
    {
        PlayButtonPressed?.Invoke();
    }

    private void OnOptionsButtonPressed()
    {
        OptionsButtonPressed?.Invoke();
    }

    private void OnHideMenuButtonPressed()
    {
        HideMenuButtonPressed?.Invoke();
    }
}