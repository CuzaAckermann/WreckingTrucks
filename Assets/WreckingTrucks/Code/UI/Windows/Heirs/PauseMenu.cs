using System;
using UnityEngine;

public class PauseMenu : Window
{
    [SerializeField] private MainMenuButton _mainMenuButton;
    [SerializeField] private ReturnButton _returnButton;
    [SerializeField] private ResetLevelButton _resetLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ReturnButtonPressed;
    public event Action ResetLevelButtonPressed;

    private void OnEnable()
    {
        _mainMenuButton.MainMenuButtonPressed += OnMainMenuButtonPressed;
        _returnButton.ReturnButtonPressed += OnReturnButtonPressed;
        _resetLevelButton.ResetLevelButtonPressed += OnResetLevelButtonPressed;
    }

    private void OnDisable()
    {
        _mainMenuButton.MainMenuButtonPressed -= OnMainMenuButtonPressed;
        _returnButton.ReturnButtonPressed -= OnReturnButtonPressed;
        _resetLevelButton.ResetLevelButtonPressed -= OnResetLevelButtonPressed;
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