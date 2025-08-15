using System;
using UnityEngine;

public class EndLevelWindow : WindowOfState<EndLevelState>
{
    [Header("Main Buttons")]
    [SerializeField] private GameButton _mainMenuButton;
    [SerializeField] private GameButton _resetLevelButton;

    [Header("Level Buttons")]
    [SerializeField] private GameButton _levelSelectionButton;
    [SerializeField] private GameButton _previousLevelButton;
    [SerializeField] private GameButton _nextLevelButton;

    public event Action MainMenuButtonPressed;
    public event Action ResetLevelButtonPressed;

    public event Action LevelSelectionButtonPressed;
    public event Action PreviousLevelButtonPressed;
    public event Action NextLevelButtonPressed;

    public void SetLevelNavigationState(bool hasNextLevel, bool hasPreviousLevel)
    {
        _nextLevelButton.gameObject.SetActive(hasNextLevel);
        _previousLevelButton.gameObject.SetActive(hasPreviousLevel);
    }

    protected override void SubscribeToInteractables(EndLevelState endLevelState)
    {
        _mainMenuButton.Pressed += OnMainMenuButtonPressed;
        _resetLevelButton.Pressed += OnResetLevelButtonPressed;

        _levelSelectionButton.Pressed += OnLevelSelectionButtonPressed;
        _previousLevelButton.Pressed += OnPreviousLevelButtonPressed;
        _nextLevelButton.Pressed += OnNextLevelButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(EndLevelState endLevelState)
    {
        _mainMenuButton.Pressed -= OnMainMenuButtonPressed;
        _resetLevelButton.Pressed -= OnResetLevelButtonPressed;

        _levelSelectionButton.Pressed -= OnLevelSelectionButtonPressed;
        _previousLevelButton.Pressed -= OnPreviousLevelButtonPressed;
        _nextLevelButton.Pressed -= OnNextLevelButtonPressed;
    }

    private void OnMainMenuButtonPressed()
    {
        MainMenuButtonPressed?.Invoke();
    }

    private void OnResetLevelButtonPressed()
    {
        ResetLevelButtonPressed?.Invoke();
    }

    private void OnLevelSelectionButtonPressed()
    {
        LevelSelectionButtonPressed?.Invoke();
    }

    private void OnPreviousLevelButtonPressed()
    {
        PreviousLevelButtonPressed?.Invoke();
    }

    private void OnNextLevelButtonPressed()
    {
        NextLevelButtonPressed?.Invoke();
    }
}