using System;
using UnityEngine;

public class BackgroundGameWindow : WindowOfState<BackgroundGameState>
{
    [SerializeField] private GameButton _showMainMenuButton;

    public event Action ShowMainMenuButtonPressed;

    protected override void SubscribeToInteractables(BackgroundGameState backgroundGameState)
    {
        _showMainMenuButton.Pressed += OnShowMainMenuButtonPressed;
    }

    protected override void UnsubscribeFromInteractables(BackgroundGameState backgroundGameState)
    {
        _showMainMenuButton.Pressed -= OnShowMainMenuButtonPressed;
    }

    private void OnShowMainMenuButtonPressed()
    {
        ShowMainMenuButtonPressed?.Invoke();
    }
}