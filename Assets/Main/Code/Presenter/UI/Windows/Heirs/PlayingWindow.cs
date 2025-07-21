using System;
using UnityEngine;

public class PlayingWindow : Window<PlayingState>
{
    [SerializeField] private GameButton _pauseButton;
    [SerializeField] private GameButton _swapAbilityButton;

    public event Action PauseButtonPressed;
    public event Action SwapAbilityButtonPressed;

    protected override void SubscribeToInteractables(PlayingState playingState)
    {
        _pauseButton.Pressed += OnPauseButtonPressed;
        _swapAbilityButton.Pressed += OnSwapAbilityButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(PlayingState playingState)
    {
        _pauseButton.Pressed -= OnPauseButtonPressed;
        _swapAbilityButton.Pressed -= OnSwapAbilityButtonPressed;
    }

    private void OnPauseButtonPressed()
    {
        PauseButtonPressed?.Invoke();
    }

    private void OnSwapAbilityButtonPressed()
    {
        SwapAbilityButtonPressed?.Invoke();
    }
}