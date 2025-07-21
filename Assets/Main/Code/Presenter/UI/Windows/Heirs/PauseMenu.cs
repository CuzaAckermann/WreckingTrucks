using System;
using UnityEngine;

public class PauseMenu : Window<PausedState>
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _returnButton;
    [SerializeField] private GameButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ReturnButtonPressed;
    public event Action ResetLevelButtonPressed;

    protected override void SubscribeToInteractables(PausedState pausedState)
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _returnButton.Pressed += OnReturnButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(PausedState pausedState)
    {
        _mainMenuButton.Pressed -= OnMainMenuButtonPressed;
        _returnButton.Pressed -= OnReturnButtonPressed;
        _resetLevelButton.Pressed -= OnResetLevelButtonPressed;
    }

    private void OnMainMenuButtonPressed()
    {
        MainMenuButtonPressed?.Invoke();
    }

    private void OnReturnButtonPressed()
    {
        ReturnButtonPressed?.Invoke();
    }

    private void OnResetLevelButtonPressed()
    {
        ResetLevelButtonPressed?.Invoke();
    }
}