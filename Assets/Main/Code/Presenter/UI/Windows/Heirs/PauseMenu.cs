using System;
using UnityEngine;

public class PauseMenu : WindowOfState<PausedState>
{
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _returnButton;
    [SerializeField] private GameButton _resetLevelButton;
    [SerializeField] private GameButton _levelSelectionButton;

    public event Action MainMenuButtonPressed;
    public event Action ReturnButtonPressed;
    public event Action ResetLevelButtonPressed;
    public event Action LevelSelectionButtonPressed;

    protected override void SubscribeToInteractables(PausedState pausedState)
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _returnButton.Pressed += OnReturnButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;
        _levelSelectionButton.Pressed += OnLevelSelectionButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(PausedState pausedState)
    {
        _mainMenuButton.Pressed -= OnMainMenuButtonPressed;
        _returnButton.Pressed -= OnReturnButtonPressed;
        _resetLevelButton.Pressed -= OnResetLevelButtonPressed;
        _levelSelectionButton.Pressed -= OnLevelSelectionButtonPressed;
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

    private void OnLevelSelectionButtonPressed()
    {
        LevelSelectionButtonPressed?.Invoke();
    }
}