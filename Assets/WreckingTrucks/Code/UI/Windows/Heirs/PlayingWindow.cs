using System;
using UnityEngine;

public class PlayingWindow : Window
{
    [SerializeField] private GameButton _pauseButton;

    public event Action PauseButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _pauseButton.Pressed += OnPauseButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _pauseButton.Pressed -= OnPauseButtonPressed;
    }

    private void OnPauseButtonPressed()
    {
        PauseButtonPressed?.Invoke();
    }
}