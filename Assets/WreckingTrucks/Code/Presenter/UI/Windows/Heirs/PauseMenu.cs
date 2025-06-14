using System;
using UnityEngine;

public class PauseMenu : Window
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _returnButton;
    [SerializeField] private GameButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ReturnButtonPressed;
    public event Action ResetLevelButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _returnButton.Pressed += OnReturnButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
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