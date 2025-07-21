using System;
using UnityEngine;

public class EndLevelWindow : Window<EndLevelState>
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ResetLevelButtonPressed;

    protected override void SubscribeToInteractables(EndLevelState endLevelState)
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(EndLevelState endLevelState)
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