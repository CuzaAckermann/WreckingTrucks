using System;
using UnityEngine;

public class PlayingWindow : Window
{
    [SerializeField] private PauseButton _pauseButton;

    public event Action PauseButtonPressed;

    private void OnEnable()
    {
        _pauseButton.PauseButtonPressed += OnPauseButtonPressed;
    }

    private void OnDisable()
    {
        _pauseButton.PauseButtonPressed -= OnPauseButtonPressed;
    }

    private void OnPauseButtonPressed()
    {
        PauseButtonPressed?.Invoke();
    }
}