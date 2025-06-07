using System;
using UnityEngine;

public class MainMenu : Window
{
    [SerializeField] private GameButton _playButton;
    [SerializeField] private GameButton _optionsButton;

    public event Action PlayButtonPressed;
    public event Action OptionsButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _playButton.Pressed += OnPlayButtonPressed;
        _optionsButton.Pressed += OnOptionsButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _playButton.Pressed -= OnPlayButtonPressed;
        _optionsButton.Pressed -= OnOptionsButtonPressed;
    }

    private void OnPlayButtonPressed()
    {
        PlayButtonPressed?.Invoke();
    }

    private void OnOptionsButtonPressed()
    {
        OptionsButtonPressed?.Invoke();
    }
}