using System;
using UnityEngine;

public class EndLevelWindow : Window
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ResetLevelButtonPressed;

    protected override void SubscribeToInteractables()
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;
    }

    protected override void UnsubscribeFromInteractables()
    {
        _mainMenuButton.Pressed -= OnMainMenuButtonPressed;
        _resetLevelButton.Pressed -= OnResetLevelButtonPressed;
    }

    private void OnMainMenuButtonPressed()
    {
        MainMenuButtonPressed?.Invoke();
    }

    private void OnResetLevelButtonPressed()
    {
        ResetLevelButtonPressed?.Invoke();
    }
}