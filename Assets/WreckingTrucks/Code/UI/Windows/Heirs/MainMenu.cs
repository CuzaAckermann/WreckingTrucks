using System;
using UnityEngine;

public class MainMenu : Window
{
    [SerializeField] private PlayButton _playButton;
    [SerializeField] private OptionsButton _optionsButton;

    public event Action PlayButtonPressed;
    public event Action OptionsButtonPressed;

    private void OnEnable()
    {
        _playButton.PlayButtonPressed += OnPlayButtonPressed;
        _optionsButton.OptionsButtonPressed += OnOptionsButtonPressed;
    }

    private void OnDisable()
    {
        _playButton.PlayButtonPressed -= OnPlayButtonPressed;
        _optionsButton.OptionsButtonPressed -= OnOptionsButtonPressed;
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