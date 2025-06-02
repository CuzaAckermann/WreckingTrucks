using System;
using UnityEngine;

public class EndLevelWindow : Window
{
    [SerializeField] private MainMenuButton _mainMenuButton;
    [SerializeField] private ResetLevelButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ResetLevelButtonPressed;

    private void OnEnable()
    {
        _mainMenuButton.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _resetLevelButton.ResetLevelButtonPressed += OnResetLevelButtonPressed;
    }

    private void OnDisable()
    {
        _mainMenuButton.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _resetLevelButton.ResetLevelButtonPressed -= OnResetLevelButtonPressed;
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